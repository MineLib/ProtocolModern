using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server.Status
{
    public struct PingPacket : IPacket
    {
        public long Time;

        public byte ID { get { return 0x01; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Time = reader.ReadLong();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteLong(Time);
            
            return this;
        }
    }
}