
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;
using Aragas.Core.IO;

namespace MineLib.PacketBuilder.Server.Handshaking
{
    public class HandshakePacket : ProtobufPacket
    {
		public VarInt ProtocolVersion;
		public String ServerAddress;
		public UInt16 ServerPort;
		public VarInt NextState;

        public override VarInt ID { get { return 0; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			ProtocolVersion = reader.Read(ProtocolVersion);
			ServerAddress = reader.Read(ServerAddress);
			ServerPort = reader.Read(ServerPort);
			NextState = reader.Read(NextState);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(ProtocolVersion);
			stream.Write(ServerAddress);
			stream.Write(ServerPort);
			stream.Write(NextState);
          
            return this;
        }

    }
}