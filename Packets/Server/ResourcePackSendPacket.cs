using MineLib.Core;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;
using MineLib.Core.Wrappers;

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
            stream.WriteString(URL);
            stream.WriteString(Hash);
            
            return this;
        }
    }
}
