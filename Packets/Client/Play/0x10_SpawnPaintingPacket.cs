
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Client.Play
{
    public class SpawnPaintingPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public String Title;
		public Position Location;
		public Byte Direction;

        public override VarInt ID { get { return 16; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Title = reader.Read(Title);
			Location = reader.Read(Location);
			Direction = reader.Read(Direction);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Title);
			stream.Write(Location);
			stream.Write(Direction);
          
            return this;
        }

    }
}