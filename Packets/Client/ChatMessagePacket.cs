using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct ChatMessagePacket : IPacket
    {
        public string Message;

        public byte ID { get { return 0x01; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Message = reader.ReadString();
            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Message);
            stream.Purge();

            return this;
        }
    }
}