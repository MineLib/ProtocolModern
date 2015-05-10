using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client.Status
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
            stream.WriteVarInt(ID);
            stream.WriteLong(Time);
            
            return this;
        }

    }
}
