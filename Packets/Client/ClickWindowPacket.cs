using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct ClickWindowPacket : IPacket
    {
        public sbyte WindowID { get; set; }
        public short Slot { get; set; }
        public sbyte Button { get; set; }
        public short ActionNumber { get; set; }
        public sbyte Mode { get; set; }
        public ItemStack ClickedItem { get; set; }

        public byte ID { get { return 0x0E; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowID = reader.ReadSByte();
            Slot = reader.ReadShort();
            Button = reader.ReadSByte();
            ActionNumber = reader.ReadShort();
            Mode = reader.ReadSByte();
            ClickedItem = ItemStack.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteSByte(WindowID);
            stream.WriteShort(Slot);
            stream.WriteSByte(Button);
            stream.WriteShort(ActionNumber);
            stream.WriteSByte(Mode);
            ClickedItem.ToStream(stream);
            
            return this;
        }
    }
}