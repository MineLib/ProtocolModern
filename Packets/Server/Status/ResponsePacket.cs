using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server.Status
{
    public struct ResponsePacket : IPacket
    {
        public string Response;

        public byte ID { get { return 0x00; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Response = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Response);
            stream.Purge();

            return this;
        }
    }
}