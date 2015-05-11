using MineLib.Core;
using MineLib.Core.IO;

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
            
            return this;
        }
    }
}