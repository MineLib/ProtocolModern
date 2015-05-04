using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct ResourcePackSendPacket : IPacket
    {
        public string URL;
        public string Hash;

        public byte ID { get { return 0x48; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            URL = reader.ReadString();
            Hash = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(URL);
            stream.WriteString(Hash);
            stream.Purge();

            return this;
        }
    }
}
