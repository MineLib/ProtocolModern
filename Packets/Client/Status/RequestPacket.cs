using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client.Status
{
    public struct RequestPacket : IPacket
    {
        public byte ID { get { return 0x00; } }

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
