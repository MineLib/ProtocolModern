using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using MineLib.Core;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;
using MineLib.Core.Wrappers;

using Newtonsoft.Json;

using PCLStorage;

using ProtocolModern.Enum;
using ProtocolModern.IO;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client.Login;
using ProtocolModern.Packets.Server;
using ProtocolModern.Packets.Server.Login;

namespace ProtocolModern
{
    public sealed partial class Protocol : IProtocol
    {
        #region Properties

        public string Name { get { return "Modern"; } }
        public string Version { get { return "1.8.7"; } }

        public ConnectionState State { get; private set; }

        public bool Connected { get { return Stream != null && Stream.Connected; } }

        public bool UseLogin { get { return Minecraft.UseLogin; } }

        public bool IsForge { get; private set; }

        // -- Debugging
        public bool SavePackets { get; private set; }
        public bool SavePacketsToDisk { get; private set; }
        public bool SaveEntityPacketsToDisk { get; private set; }
        public bool SaveRawMapPacketsToDisk { get; private set; }

        public List<IPacket> PacketsReceived { get; private set; }
        public List<IPacket> PacketsSended { get; private set; }
        public List<IPacket> PluginMessage { get; private set; }

        public List<IPacket> LastPackets
        {
            get
            {
                if (PacketsReceived != null)
                    return PacketsReceived.GetRange(PacketsReceived.Count - 50, 50);
                else
                    return null;
            }
        }
        public IPacket LastPacket { get { return PacketsReceived[PacketsReceived.Count - 1]; } }

        private IFolder PacketDumpFolder { get; set; }
        private IFolder PacketDumpRawFolder { get; set; }
        // -- Debugging

        #endregion

        private int ReadThreadID { get; set; }
        private CancellationTokenSource CancellationToken { get; set; }

        private IMinecraftClient Minecraft { get; set; }

        private IProtocolStreamExtended Stream { get; set; }

        private bool CompressionEnabled { get { return Stream != null && Stream.ModernCompressionEnabled; } }
        private long CompressionThreshold { get { return Stream == null ? -1 : Stream.ModernCompressionThreshold; } }


        public IProtocol Initialize(IMinecraftClient client, bool debugPackets = false)
        {
            Minecraft = client;
            Stream = new ModernStream(NetworkTCPWrapper.NewNetworkTcp());
            SavePackets = debugPackets;
            SavePacketsToDisk       = true;
            SaveEntityPacketsToDisk = false;
            SaveRawMapPacketsToDisk = false;

            PacketsReceived = new List<IPacket>();
            PacketsSended = new List<IPacket>();
            PluginMessage = new List<IPacket>();

            SendingAsyncHandlers = new Dictionary<Type, List<Func<SendingArgs, Task>>>();
            CustomPacketHandlers = new Dictionary<Type, List<Func<IPacket, Task>>>();
            RegisterSupportedSendings();

            CancellationToken = new CancellationTokenSource();

            var time = DateTime.Now;
            PacketDumpFolder = FileSystemWrapper.LogFolder.CreateFolderAsync(string.Format("{0:yyyy-MM-dd_hh.mm.ss}", time), CreationCollisionOption.GenerateUniqueName).Result;
            PacketDumpRawFolder = FileSystemWrapper.LogFolder.CreateFolderAsync(string.Format("{0:yyyy-MM-dd_hh.mm.ss}-Raw", time), CreationCollisionOption.GenerateUniqueName).Result;

            return this;
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
                    var dataLengthBytes = ModernStream.GetVarIntBytes(dataLength).Length;

                    var tempBuff = Stream.ReadByteArray(packetLength - dataLengthBytes); // -- Compressed

                    using (var outputStream = new MemoryStream())
                    using (var inputStream = new InflaterInputStream(new MemoryStream(tempBuff)))
                    //using (var reader = new ModernDataReader(new MemoryStream(tempBuff))) // -- For VarInt
                    {
                        inputStream.CopyTo(outputStream);
                        tempBuff = outputStream.ToArray(); // -- Decompressed

                        packetId = tempBuff[0]; // -- Only 255 packets available. ReadVarInt doesn't work.
                        var packetIdBytes = ModernStream.GetVarIntBytes(packetId).Length;

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
            using (var reader = new ModernDataReader(data))
            {
                var packet = default(IPacket);

                switch (State)
                {
                    #region Status

                    case ConnectionState.InfoRequest:
                        if (ServerResponse.InfoRequest[id] == null)
                            throw new ProtocolException("Reading error: Wrong Status packet ID.");

                        packet = ServerResponse.InfoRequest[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.InfoRequest);
                        break;

                    #endregion Status

                    #region Login

                    case ConnectionState.Joining:
                        if (ServerResponse.JoiningServer[id] == null)
                            throw new ProtocolException("Reading error: Wrong Login packet ID.");

                        packet = ServerResponse.JoiningServer[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.Joining);
                        break;

                    #endregion Login

                    #region Play

                    case ConnectionState.Joined:
                        if (ServerResponse.JoinedServer[id] == null)
                            throw new ProtocolException("Reading error: Wrong Play packet ID.");

                        packet = ServerResponse.JoinedServer[id]().ReadPacket(reader);

                        OnPacketHandled(id, packet, ConnectionState.Joined);
                        break;

                    #endregion Play
                }

                if(packet is PluginMessagePacket)
                    PluginMessage.Add(packet);

                if (SavePackets)
                    PacketsReceived.Add(packet);

                if (SavePacketsToDisk)
                    DumpPacketReceived(packet, data);
            }
        }
        
        private int _rCount = 0;
        private readonly Dictionary<short, int> _rCounters = new Dictionary<short, int>(); 
        private void DumpPacketReceived(IPacket packet, byte[] data)
        {
            if(!_rCounters.ContainsKey(packet.ID))
                _rCounters.Add(packet.ID, 0);

            var mainData = string.Format("{0}{1}{2}", "R" + _rCount, packet.GetType().Name.Replace("Packet", ""), _rCounters[packet.ID]);



            if (!SaveEntityPacketsToDisk)
                if(packet is EntityHeadLookPacket || packet is EntityLookPacket || packet is EntityRelativeMovePacket || packet is EntityLookAndRelativeMovePacket ||
                    packet is EntityVelocityPacket || packet is EntityTeleportPacket || packet is SpawnMobPacket || packet is DestroyEntitiesPacket ||
                    packet is EntityStatusPacket || packet is EntityMetadataPacket || packet is EntityPropertiesPacket || packet is EntityEquipmentPacket || packet is AttachEntityPacket)
                { _rCount++; return; }

            var name = string.Format("{0}.json", mainData);
            using (var stream = PacketDumpFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new StreamWriter(stream))
                writer.Write(JsonConvert.SerializeObject(packet, Formatting.Indented, new[] { new VarIntJsonConverter() } ));



            if (!SaveRawMapPacketsToDisk)
                if (packet is MapChunkBulkPacket || packet is ChunkDataPacket)
                { _rCount++; return; }

            var rawName = string.Format("{0}.bin", mainData);
            using (var stream = PacketDumpRawFolder.CreateFileAsync(rawName, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new BinaryWriter(stream))
                writer.Write(data);
            


            _rCounters[packet.ID]++;
            _rCount++;
        }

        private int _sCount = 0;
        private readonly Dictionary<short, int> _sCounters = new Dictionary<short, int>();
        private void DumpPacketSended(IPacket packet)
        {
            if (!_sCounters.ContainsKey(packet.ID))
                _sCounters.Add(packet.ID, 0);

            var name = string.Format("{0}{1}{2}.json", "S" + _sCount, packet.GetType().Name.Replace("Packet", ""), _sCounters[packet.ID]);
            using (var stream = PacketDumpFolder.CreateFileAsync(name, CreationCollisionOption.OpenIfExists).Result.OpenAsync(FileAccess.ReadAndWrite).Result)
            using (var writer = new StreamWriter(stream))
                writer.Write(JsonConvert.SerializeObject(packet, Formatting.Indented, new[] { new VarIntJsonConverter() }));

            _sCounters[packet.ID]++;
            _sCount++;
        }


        /// <summary>
        /// Enabling encryption
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private async void ModernEnableEncryption(IPacket packet)
        {
            var request = (EncryptionRequestPacket) packet;
            var sharedKey = PKCS1Signature.CreateSecretKey();

            var hash = GetServerIDHash(request.PublicKey, sharedKey, request.ServerId);

            if (!Yggdrasil.JoinSession(Minecraft.AccessToken, Minecraft.SelectedProfile, hash).Result)
                throw new ProtocolException("Yggdrasil error: Not authenticated.");
            
            var pkcs = new PKCS1Signature(request.PublicKey);
            var signedSecret = pkcs.SignData(sharedKey);
            var signedVerify = pkcs.SignData(request.VerificationToken);

            await SendPacketAsync(new EncryptionResponsePacket
            {
                SharedSecret = signedSecret,
                VerificationToken = signedVerify
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
        private void ModernSetCompression(IPacket packet)
        {
            var request = (ISetCompressionPacket) packet;

            Stream.SetCompression(request.Threshold);
        }


        #region Network


        public void Connect(string host, ushort port)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            // -- Connect to server.
            Stream.Connect(host, port);

            // -- Begin data reading.
            if (ThreadWrapper.IsRunning(ReadThreadID))
                throw new ProtocolException("Connection error: Thread already running.");
            else
                ReadThreadID = ThreadWrapper.StartThread(ReadCycle, true, "PacketReaderThread");
        }

        public void Disconnect()
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            CancellationToken.Cancel();

            Stream.Disconnect();
        }

        private void SendPacket(IPacket packet)
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


        public async Task ConnectAsync(string host, ushort port)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            await Stream.ConnectAsync(host, port);

            if (ThreadWrapper.IsRunning(ReadThreadID))
                throw new ProtocolException("Connection error: Thread already running.");
            else
                ReadThreadID = ThreadWrapper.StartThread(ReadCycle, true, "PacketReaderThread");
        }
        
        public bool DisconnectAsync()
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            CancellationToken.Cancel();

            return Stream.DisconnectAsync();
        }

        private async Task SendPacketAsync(IPacket packet)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            HandleState(packet);

            await Stream.SendPacketAsync(packet);

            if (SavePackets)
                PacketsSended.Add(packet);

            if (SavePacketsToDisk)
                DumpPacketSended(packet);
        }


        private void HandleState(IPacket packet)
        {
            var handshake = packet as HandshakePacket;
            if (handshake != null)
            {
                switch (handshake.NextState)
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


        public void Dispose()
        {
            if (CancellationToken != null)
                CancellationToken.Cancel();
            
            if (_rCounters != null)
                _rCounters.Clear();

            if (_sCounters != null)
                _sCounters.Clear();

            if (Stream != null)
                Stream.Dispose();

            if (PacketsReceived != null)
                PacketsReceived.Clear();

            if (PacketsSended != null)
                PacketsSended.Clear();

            if (CancellationToken != null)
                CancellationToken.Dispose();
        }
    }
}
