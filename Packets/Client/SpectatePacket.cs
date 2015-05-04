using MineLib.Network;
using MineLib.Network.IO;
using Org.BouncyCastle.Math;

namespace ProtocolModern.Packets.Client
{
    public struct SpectatePacket : IPacket
    {
        public BigInteger UUID;

        public byte ID { get { return 0x18; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            UUID = reader.ReadBigInteger();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteBigInteger(UUID);
            stream.Purge();

            return this;
        }
    }
}
