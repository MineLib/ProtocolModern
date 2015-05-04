using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct ClientStatusPacket : IPacket
    {
        public ClientStatus Status;

        public byte ID { get { return 0x16; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Status = (ClientStatus) reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Status);
            stream.Purge();

            return this;
        }
    }
}