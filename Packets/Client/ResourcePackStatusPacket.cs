using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct ResourcePackStatusPacket : IPacket
    {
        public string Hash { get; set; }
        public ResourcePackStatus Result { get; set; }

        public byte ID { get { return 0x19; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Hash = reader.ReadString();
            Result = (ResourcePackStatus) (int) reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteString(Hash);
            stream.WriteVarInt((int) Result);
            
            return this;
        }
    }
}
