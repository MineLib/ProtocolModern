using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct CreativeInventoryActionPacket : IPacket
    {
        public short Slot { get; set; }
        public ItemStack ClickedItem { get; set; }

        public byte ID { get { return 0x10; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Slot = reader.ReadShort();
            ClickedItem = ItemStack.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteShort(Slot);
            ClickedItem.ToStream(stream);
            
            return this;
        }
    }
}