using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct KeepAlivePacket : IPacket
    {
        public VarInt KeepAlive { get; set; }

        public byte ID { get { return 0x00; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            KeepAlive = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(KeepAlive);
            
            return this;
        }
    }
}