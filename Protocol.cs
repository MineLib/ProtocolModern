using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Packets;
using Aragas.Core.Wrappers;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using MineLib.Core;
using MineLib.Core.Events;
using MineLib.Core.Exceptions;
using MineLib.Core.IO;

using MineLib.PacketBuilder.Client.Login;
using MineLib.PacketBuilder.Client.Play;
using MineLib.PacketBuilder.Server.Handshaking;
using MineLib.PacketBuilder.Server.Login;

using Newtonsoft.Json;

using PCLStorage;

using ProtocolModern.Enum;
using ProtocolModern.IO;
using ProtocolModern.Packets;

namespace ProtocolModern
{
    public sealed partial class Protocol : MineLib.Core.Protocols.Protocol
    {
        static Protocol()
        {
            Extensions.PacketExtensions.Init();
        }

        #region Properties

        public override string Name => "Modern";
        public override string Version => "1.8.7";

        public override ConnectionState State { get; protected set; }

        public override string Host => Stream?.Host;
        public override ushort Port => Stream?.Port ?? 0;
        public override bool Connected => Stream != null && Stream.Connected;

        public override bool UseLogin => Minecraft.UseLogin;

        private bool IsForge { get; set; }

        #endregion Properties


        #region Debugging

        public override bool SavePackets { get; }
        private bool SavePacketsToDisk { get; set; }
        private bool SaveEntityPacketsToDisk { get; set; }
        private bool SaveRawMapPacketsToDisk { get; set; }

        protected override List<ProtobufPacket> PacketsReceived { get; }
        protected override List<ProtobufPacket> PacketsSended { get; }
        private List<ProtobufPacket> PluginMessage { get; }

        protected override List<ProtobufPacket> LastPackets => PacketsReceived?.GetRange(PacketsReceived.Count - 50, 50);
        protected override ProtobufPacket LastProtobufPacket => PacketsReceived[PacketsReceived.Count - 1];

        private IFolder PacketDumpFolder { get; set; }
        private IFolder PacketDumpRawFolder { get; set; }

        #endregion Debugging

        private IThread ReadThread { get; set; }
        private CancellationTokenSource CancellationToken { get; set; }

        private MineLibClient Minecraft { get; set; }

        private CompressionProtobufStream Stream { get; set; }

        private bool CompressionEnabled => Stream != null && Stream.CompressionEnabled;
        private long CompressionThreshold => Stream?.CompressionThreshold ?? -1;


        public Protocol(MineLibClient client, bool debugPackets = false) : base(client, debugPackets)
        {
            Minecraft = client;
            Stream = new CompressionProtobufStream(TCPClientWrapper.CreateTCPClient());

            SavePackets             = debugPackets;
            SavePacketsToDisk       = true;
            SaveEntityPacketsToDisk = false;
            SaveRawMapPacketsToDisk = false;

            PacketsReceived = new List<ProtobufPacket>();
            PacketsSended = new List<ProtobufPacket>();
            PluginMessage = new List<ProtobufPacket>();

            SendingHandlers = new Dictionary<Type, List<Action<SendingEventArgs>>>();
            CustomPacketHandlers = new Dictionary<Type, List<Func<ProtobufPacket, Task>>>();
            RegisterSupportedSendings();

            CancellationToken = new CancellationTokenSource();

            var time = DateTime.Now;
            PacketDumpFolder = FileSystemWrapper.LogFolder.CreateFolderAsync($"{time:yyyy-MM-dd_hh.mm.ss}", CreationCollisionOption.GenerateUniqueName).Result;
            PacketDumpRawFolder = FileSystemWrapper.LogFolder.CreateFolderAsync($"{time:yyyy-MM-dd_hh.mm.ss}-Raw", CreationCollisionOption.GenerateUniqueName).Result;
        }


        private void ReadCycle()
        {
            while (!CancellationToken.IsCancellationRequested)
                PacketReceiver();
        }
        private void PacketReceiver()
        {
            int packetId;
            byte[] data;

            #region No Compression

            if (!CompressionEnabled)
            {
                var packetLength = Stream.ReadVarInt();
                if (packetLength == 0)
                    throw new ProtocolException("Reading error: Packet Length size is 0");

                packetId = Stream.ReadVarInt();

                data = Stream.ReadByteArray(packetLength - 1);
            }

            #endregion

            #region Compression

            else // (CompressionEnabled)
            {
                // REWRITE: Blocking thread
                var packetLength = Stream.ReadVarInt();
                if (packetLength == 0)
                {
                    if (CancellationToken.Token.IsCancellationRequested)
                        return;
                    else
                        throw new ProtocolException("Reading error: Packet Length size is 0");
                }

                var dataLength = Stream.ReadVarInt();
                if (dataLength == 0)
                {
                    if (packetLength > CompressionThreshold)
                    { 
                        if (CancellationToken.Token.IsCancellationRequested)
                            return;
                        else
                            throw new ProtocolException("Reading error: Received uncompressed message of size " + packetLength + " greater than threshold " + CompressionThreshold);
                    }

                    packetId = Stream.ReadVarInt();

                    //data = _stream.ReadByteArray(packetLength - ModernStream.GetVarIntBytes(packetId).Length);
                    data = Stream.ReadByteArray(packetLength - 2);
                }
                else // (dataLength > 0)
                {
                    var dataLengthBytes = new VarInt(dataLength).InByteArray().Length;

                    var tempBuff = Stream.ReadByteArray(packetLength - dataLengthBytes); // -- Compressed

                    using (var outputStream = new MemoryStream())
                    using (var inputStream = new InflaterInputStream(new MemoryStream(tempBuff)))
                    //using (var reader = new ModernDataReader(new MemoryStream(tempBuff))) // -- For VarInt
                    {
                        inputStream.CopyTo(outputStream);
                        tempBuff = outputStream.ToArray(); // -- Decompressed

                        packetId = tempBuff[0]; // -- Only 255 packets available. ReadVarInt doesn't work.
                        var packetIdBytes = new VarInt(packetId).InByteArray().Length;

                        data = new byte[tempBuff.Length - packetIdBytes];
                        Buffer.BlockCopy(tempBuff, packetIdBytes, data, 0, data.Length);
                    }
                }
            }

            #endregion

            HandlePacket(packetId, data);
        }


        /// <summary>
        /// Packets are handled here. Compression and encryption are handled here too
        /// </summary>
        /// <param name="id">Packet ID</param>
        /// <param name="data">Packet byte[] data</param>
        private void HandlePacket(int id, byte[] data)
        {
            using (var reader = new ProtobufDataReader(data))
            {
                var packet = default(ProtobufPacket);

                switch (State)
                {
                    #region Status

                    case ConnectionState.InfoRequest:
                        if (ClientResponse.StatusPacketResponses.Packets[id] == null)
                            throw new ProtocolException("Reading error: Wrong Status packet ID.");

                        packet = ClientResponse.StatusPacketResponses.Packets[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.InfoRequest);
                        break;

                    #endregion Status

                    #region Login

                    case ConnectionState.Joining:
                        if (ClientResponse.LoginPacketResponses.Packets[id] == null)
                            throw new ProtocolException("Reading error: Wrong Login packet ID.");

                        packet = ClientResponse.LoginPacketResponses.Packets[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.Joining);
                        break;

                    #endregion Login

                    #region Play

                    case ConnectionState.Joined:
                        if (ClientResponse.PlayPacketResponses.Packets[id] == null)
                        {
                            break;
                            throw new ProtocolException("Reading error: Wrong Play packet ID.");
                        }

                        packet = ClientResponse.PlayPacketResponses.Packets[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.Joined);
                        break;

                    #endregion Play
                }

                if(packet == null)
                    return;

                if(packet is PluginMessagePacket)
                    PluginMessage.Add(packet);

                if (SavePackets)
                    PacketsReceived.Add(packet);

                if (SavePacketsToDisk)
                    DumpPacketReceived(packet, data);
            }
        }
        
        private int _rCount;
        private readonly Dictionary<short, int> _rCounters = new Dictionary<short, int>(); 
        private void DumpPacketReceived(ProtobufPacket packet, byte[] data)
        {
            if(!_rCounters.ContainsKey((short) packet.ID))
                _rCounters.Add((short) packet.ID, 0);

            var mainData = $"R{_rCount}{packet.GetType().Name.Replace("Packet", "")}{_rCounters[(short) packet.ID]}";



            if (!SaveEntityPacketsToDisk)
                if(packet is EntityHeadLookPacket || packet is EntityLookPacket || packet is EntityRelativeMovePacket || packet is EntityLookAndRelativeMovePacket ||
                    packet is EntityVelocityPacket || packet is EntityTeleportPacket || packet is SpawnMobPacket || packet is DestroyEntitiesPacket ||
                    packet is EntityStatusPacket || packet is EntityMetadataPacket || packet is EntityPropertiesPacket || packet is EntityEquipmentPacket || packet is AttachEntityPacket)
                { _rCount++; return; }

            var name = $"{mainData}.json";
            using (var stream = PacketDumpFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new StreamWriter(stream))
                writer.Write(JsonConvert.SerializeObject(packet, Formatting.Indented, new VarIntJsonConverter()));



            if (!SaveRawMapPacketsToDisk)
                if (packet is MapChunkBulkPacket || packet is ChunkDataPacket)
                { _rCount++; return; }

            var rawName = $"{mainData}.bin";
            using (var stream = PacketDumpRawFolder.CreateFileAsync(rawName, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new BinaryWriter(stream))
                writer.Write(data);
            


            _rCounters[(short) packet.ID]++;
            _rCount++;
        }

        private int _sCount;
        private readonly Dictionary<short, int> _sCounters = new Dictionary<short, int>();
        private void DumpPacketSended(ProtobufPacket packet)
        {
            if (!_sCounters.ContainsKey((short) packet.ID))
                _sCounters.Add((short) packet.ID, 0);

            var name = $"S{_sCount}{packet.GetType().Name.Replace("Packet", "")}{_sCounters[(short) packet.ID]}.json";
            using (var stream = PacketDumpFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new StreamWriter(stream))
                writer.Write(JsonConvert.SerializeObject(packet, Formatting.Indented, new VarIntJsonConverter()));

            _sCounters[(short) packet.ID]++;
            _sCount++;
        }


        /// <summary>
        /// Enabling encryption
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernEnableEncryption(ProtobufPacket packet)
        {
            var request = (EncryptionRequestPacket) packet;
            var sharedKey = PKCS1Signature.CreateSecretKey();

            var hash = GetServerIDHash(request.PublicKey, sharedKey, request.ServerID);
            if (!Yggdrasil.JoinSession(AccessToken, SelectedProfile, hash).Result)
                throw new ProtocolException("Yggdrasil error: Not authenticated.");
            
            var pkcs = new PKCS1Signature(request.PublicKey);
            SendPacket(new EncryptionResponsePacket
            {
                SharedSecret = pkcs.SignData(sharedKey),
                VerifyToken = pkcs.SignData(request.VerifyToken)
            });

            Stream.InitializeEncryption(sharedKey);
        }

        private static string GetServerIDHash(byte[] publicKey, byte[] secretKey, string serverID)
        {
            var hashlist = new List<byte>();
            hashlist.AddRange(Encoding.UTF8.GetBytes(serverID));
            hashlist.AddRange(secretKey);
            hashlist.AddRange(publicKey);

            return JavaHelper.JavaHexDigest(hashlist.ToArray());
        }


        /// <summary>
        /// Setting compression
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernSetCompression(ProtobufPacket packet)
        {
            if (packet is SetCompressionPacket)
            {
                var request = (SetCompressionPacket) packet;

                Stream.SetCompression(request.Threshold);
            }

            if (packet is SetCompression2Packet)
            {
                var request = (SetCompression2Packet)packet;

                Stream.SetCompression(request.Threshold);
            }
        }


        #region Network
        
        public override void Connect(string host, ushort port)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            // -- Connect to server.
            Stream.Connect(host, port);

            // -- Begin data reading.
            if (ReadThread != null && ReadThread.IsRunning)
                throw new ProtocolException("Connection error: Thread already running.");
            else
            {
                ReadThread = ThreadWrapper.CreateThread(ReadCycle);
                ReadThread.IsBackground = true;
                ReadThread.Name = "PacketReaderThread";
                ReadThread.Start();
            }

        }
        public override void Disconnect()
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            CancellationToken.Cancel();

            Stream.Disconnect();
        }
        private void SendPacket(ProtobufPacket packet)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            HandleState(packet);

            Stream.SendPacket(ref packet);

            if (SavePackets)
                PacketsSended.Add(packet);

            if (SavePacketsToDisk)
                DumpPacketSended(packet);
        }
        
        private void HandleState(ProtobufPacket packet)
        {
            var handshake = packet as HandshakePacket;
            if (handshake != null)
            {
                switch ((NextState)(int)handshake.NextState)
                {
                    case NextState.Status:
                        State = ConnectionState.InfoRequest;
                        break;
                    case NextState.Login:
                        State = ConnectionState.Joining;
                        break;
                }
            }
        }
        
        #endregion


        public override void Dispose()
        {
            CancellationToken?.Cancel();

            _rCounters?.Clear();
            _sCounters?.Clear();

            Stream?.Dispose();

            PacketsReceived?.Clear();
            PacketsSended?.Clear();

            //CancellationToken?.Dispose();
        }
    }
}
