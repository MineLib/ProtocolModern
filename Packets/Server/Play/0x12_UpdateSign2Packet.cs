
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Server.Play
{
    public class UpdateSign2Packet : ProtobufPacket
    {
		public Position Location;
		public String Line1;
		public String Line2;
		public String Line3;
		public String Line4;

        public override VarInt ID { get { return 18; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Location = reader.Read(Location);
			Line1 = reader.Read(Line1);
			Line2 = reader.Read(Line2);
			Line3 = reader.Read(Line3);
			Line4 = reader.Read(Line4);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Location);
			stream.Write(Line1);
			stream.Write(Line2);
			stream.Write(Line3);
			stream.Write(Line4);
          
            return this;
        }

    }
}