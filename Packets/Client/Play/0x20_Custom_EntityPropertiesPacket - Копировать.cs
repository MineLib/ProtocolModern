
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
    public class EntityPropertiesPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Int32 NumberOfProperties;
		public EntityProperty[] Property;

        public override VarInt ID { get { return 0x20; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
            EntityID = reader.Read(EntityID);
            var PropertyLength = reader.Read<Int32>();
            Property = reader.Read(Property, PropertyLength);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Property.Length);
            stream.Write(Property);

            return this;
        }

    }
}