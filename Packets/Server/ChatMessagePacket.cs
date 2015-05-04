using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;
        public ChatMessagePosition Position;

        public byte ID { get { return 0x02; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Message = reader.ReadString();
            Position = (ChatMessagePosition) reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Message);
            stream.WriteByte((byte) Position);
            stream.Purge();

            return this;
        }
    }
}