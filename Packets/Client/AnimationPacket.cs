using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct AnimationPacket : IPacket
    {
        public byte ID { get { return 0x0A; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.Purge();

            return this;
        }
    }
}