using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct EnchantItemPacket : IPacket
    {
        public sbyte WindowId { get; set; }
        public sbyte Enchantment { get; set; }

        public byte ID { get { return 0x11; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowId = reader.ReadSByte();
            Enchantment = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteSByte(WindowId);
            stream.WriteSByte(Enchantment);
            
            return this;
        }
    }
}