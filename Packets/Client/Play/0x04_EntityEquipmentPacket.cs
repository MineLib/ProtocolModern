
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
    public class EntityEquipmentPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Int16 Slot;
		public ItemStack Item;

        public override VarInt ID { get { return 4; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Slot = reader.Read(Slot);
			Item = reader.Read(Item);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Slot);
			stream.Write(Item);
          
            return this;
        }

    }
}