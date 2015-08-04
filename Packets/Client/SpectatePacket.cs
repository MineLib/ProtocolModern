using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using Org.BouncyCastle.Math;

namespace ProtocolModern.Packets.Client
{
    public struct SpectatePacket : IPacket
    {
        public BigInteger UUID { get; set; }

        public byte ID { get { return 0x18; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            UUID = reader.ReadBigInteger();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteBigInteger(UUID);
            
            return this;
        }
    }
}
