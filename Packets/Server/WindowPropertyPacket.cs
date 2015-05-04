using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct WindowPropertyPacket : IPacket
    {
        public byte WindowId;
        public short PropertyId;
        public short Value;

        public byte ID { get { return 0x31; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowId = reader.ReadByte();
            PropertyId = reader.ReadShort();
            Value = reader.ReadShort();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowId);
            stream.WriteShort(PropertyId);
            stream.WriteShort(Value);
            stream.Purge();

            return this;
        }
    }
}