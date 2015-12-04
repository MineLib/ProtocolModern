
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Server.Handshaking
{
    public class LegacyServerListPingPacket : ProtobufPacket
    {
		public Byte Payload;

        public override VarInt ID { get { return 254; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Payload = reader.Read(Payload);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Payload);
          
            return this;
        }

    }
}