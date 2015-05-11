using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct EnchantItemPacket : IPacket
    {
        public byte WindowId;
        public byte Enchantment;

        public byte ID { get { return 0x11; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowId = reader.ReadByte();
            Enchantment = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(WindowId);
            stream.WriteVarInt(Enchantment);
            
            return this;
        }
    }
}