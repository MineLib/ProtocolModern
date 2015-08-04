using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server.Login
{
    public struct LoginSuccessPacket : IPacket
    {
        public string UUID { get; set; }
        public string Username { get; set; }

        public byte ID { get { return 0x02; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            UUID = reader.ReadString();
            Username = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteString(UUID);
            stream.WriteString(Username);
            
            return this;
        }
    }
}