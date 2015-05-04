using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct EntityEquipmentPacket : IPacket
    {
        public int EntityID;
        public EntityEquipmentSlot Slot;
        public ItemStack Item;

        public byte ID { get { return 0x04; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Slot = (EntityEquipmentSlot) reader.ReadShort();
            Item = ItemStack.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteShort((short) Slot);
            Item.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}