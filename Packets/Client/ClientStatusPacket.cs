using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct ClientStatusPacket : IPacket
    {
        public ClientStatus Status { get; set; }

        public byte ID { get { return 0x16; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Status = (ClientStatus) (int) reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt((int) Status);
            
            return this;
        }
    }
}