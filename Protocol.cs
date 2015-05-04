using System;
using System.Collections.Generic;
using System.IO;

using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

using MineLib.Network;
using MineLib.Network.IO;

using ProtocolModern.IO;
using ProtocolModern.Packets;
using ProtocolModern.Packets.Client.Login;
using ProtocolModern.Packets.Server.Login;

namespace ProtocolModern
{
    public sealed partial class Protocol : IProtocol
    {
        #region Properties

        public string Name { get { return "Modern"; } }
        public string Version { get { return "1.8.4"; } }

        public ConnectionState State { get; set; }

        public bool Connected { get { return _stream != null && _stream.Connected; } }

        public bool UseLogin { get { return _minecraft.UseLogin; } }

        // -- Debugging
        public bool SavePackets { get; private set; }

        public List<IPacket> PacketsReceived { get; private set; }
        public List<IPacket> PacketsSended { get; private set; }

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
        // -- Debugging

        #endregion

        private IMinecraftClient _minecraft;

        private IProtocolStreamExtended _stream;

        private bool CompressionEnabled { get { return _stream != null && _stream.ModernCompressionEnabled; } }
        private long CompressionThreshold { get { return _stream == null ? -1 : _stream.ModernCompressionThreshold; } }


        public IProtocol Initialize(IMinecraftClient client, INetworkTCP tcp, bool debugPackets = false)
        {
            _minecraft = client;
            _stream = new ModernStream(tcp);
            SavePackets = debugPackets;

            PacketsReceived = new List<IPacket>();
            PacketsSended = new List<IPacket>();

            AsyncSendingHandlers = new Dictionary<Type, Func<IAsyncSendingArgs, IAsyncResult>>();
            RegisterSupportedAsyncSendings();

            return this;
        }

        private void PacketReceiverAsync(IAsyncResult ar)
        {
            if (!Connected)
                return; // -- Terminate cycle

            if (_stream.Available)
            {
                int packetId;
                byte[] data;

                #region No Compression

                if (!CompressionEnabled)
                {
                    var packetLength = _stream.ReadVarInt();
                    if (packetLength == 0)
                        throw new ProtocolException("Reading error: Packet Length size is 0");

                    packetId = _stream.ReadVarInt();

                    data = _stream.ReadByteArray(packetLength - 1);
                }

                #endregion

                #region Compression

                else // (CompressionEnabled)
                {
                    var packetLength = _stream.ReadVarInt();
                    if (packetLength == 0)
                        throw new ProtocolException("Reading error: Packet Length size is 0");

                    var dataLength = _stream.ReadVarInt();
                    if (dataLength == 0)
                    {
                        if (packetLength > CompressionThreshold)
                            throw new ProtocolException("Reading error: Received uncompressed message of size " + packetLength +
                                " greater than threshold " + CompressionThreshold);

                        packetId = _stream.ReadVarInt();

                        data = _stream.ReadByteArray(packetLength - 2);
                    }
                    else // (dataLength > 0)
                    {
                        var dataLengthBytes = ModernStream.GetVarIntBytes(dataLength).Length;

                        var tempBuff = _stream.ReadByteArray(packetLength - dataLengthBytes); // -- Compressed

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

            // -- If it will throw an error, then the cause is too slow _stream dispose
            _stream.EndRead(ar);
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
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
                IPacket packet = null;

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

                if (SavePackets)
                    PacketsReceived.Add(packet);
            }
        }


        /// <summary>
        /// Enabling encryption
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernEnableEncryption(IPacket packet)
        {
            var request = (EncryptionRequestPacket) packet;
            var sharedKey = PKCS15.CreateSecretKey();

            var hash = PKCS15.GetServerIDHash(request.PublicKey, sharedKey, request.ServerId);

            if (!Yggdrasil.ClientAuth(_minecraft.AccessToken, _minecraft.SelectedProfile, hash))
                throw new ProtocolException("Yggdrasil error: Not authenticated.");

            var rsaParameter = PKCS15.GetRsaKeyParameters(request.PublicKey);
            var encryptedSecret = PKCS15.EncryptData(rsaParameter, sharedKey);
            var encryptedVerify = PKCS15.EncryptData(rsaParameter, request.VerificationToken);

            BeginSendPacketHandled(new EncryptionResponsePacket
            {
                SharedSecret = encryptedSecret,
                VerificationToken = encryptedVerify
            }, null, null);

            _stream.InitializeEncryption(sharedKey);
        }

        /// <summary>
        /// Setting compression
        /// </summary>
        /// <param name="packet">EncryptionRequestPacket</param>
        private void ModernSetCompression(IPacket packet)
        {
            var request = (ISetCompression)packet;

            _stream.SetCompression(request.Threshold);
        }


        #region Network

        public IAsyncResult BeginSendPacketHandled(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            var result = BeginSendPacket(packet, asyncCallback, state);
            EndSendPacket(result);

            if (SavePackets)
                PacketsSended.Add(packet);

            return result;
        }

        public IAsyncResult BeginSendPacket(IPacket packet, AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            return _stream.BeginSendPacket(packet, asyncCallback, state);
        }

        public void EndSendPacket(IAsyncResult asyncResult)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _stream.EndSend(asyncResult);
        }

        /// <summary>
        /// If connected, don't call EndConnect.
        /// </summary>
        public IAsyncResult BeginConnect(string ip, ushort port, AsyncCallback asyncCallback, object state)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            var result = _stream.BeginConnect(ip, port, asyncCallback, state);
            EndConnect(result);
            
            return result;
        }

        private void EndConnect(IAsyncResult asyncResult)
        {
            _stream.EndConnect(asyncResult);

            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }


        public IAsyncResult BeginDisconnect(AsyncCallback asyncCallback, object state)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");
            
            return _stream.BeginDisconnect(false, asyncCallback, state);
        }

        public void EndDisconnect(IAsyncResult asyncResult)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");
            
            _stream.EndDisconnect(asyncResult);
        }


        public void SendPacket(IPacket packet)
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _stream.SendPacket(packet);

            if (SavePackets)
                PacketsSended.Add(packet);
        }

        public void Connect(string ip, ushort port)
        {
            if (Connected)
                throw new ProtocolException("Connection error: Already connected to server.");

            // -- Connect to server.
            _stream.Connect(ip, port);

            // -- Begin data reading.
            _stream.BeginRead(new byte[0], 0, 0, PacketReceiverAsync, null);
        }

        public void Disconnect()
        {
            if (!Connected)
                throw new ProtocolException("Connection error: Not connected to server.");

            _stream.Disconnect(false);
        }

        #endregion


        public void Dispose()
        {
            if (_stream != null)
                _stream.Dispose();

            if (PacketsReceived != null)
                PacketsReceived.Clear();

            if (PacketsSended != null)
                PacketsSended.Clear();
        }
    }
}
