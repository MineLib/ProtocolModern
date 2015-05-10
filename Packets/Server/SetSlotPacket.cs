using MineLib.Core;
using MineLib.Core.Data.Structs;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct SetSlotPacket : IPacket
    {
        public sbyte WindowId;
        public short Slot;
        public ItemStack SlotData;

        public byte ID { get { return 0x2F; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowId = reader.ReadSByte();
            Slot = reader.ReadShort();
            SlotData = ItemStack.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte(WindowId);
            stream.WriteShort(Slot);
            SlotData.ToStream(stream);
            
            return this;
        }
    }
}