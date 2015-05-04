using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct CreativeInventoryActionPacket : IPacket
    {
        public short Slot;
        public ItemStack ClickedItem;

        public byte ID { get { return 0x10; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Slot = reader.ReadShort();
            ClickedItem = ItemStack.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteShort(Slot);
            ClickedItem.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}