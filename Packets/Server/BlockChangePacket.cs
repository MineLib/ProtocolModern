using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct BlockChangePacket : IPacket
    {
        public Position Location { get; set; }
        public int BlockIDMeta { get; set; }

        public byte ID { get { return 0x23; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromLong(reader.ReadLong());
            BlockIDMeta = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            Location.ToStreamLong(stream);
            stream.WriteVarInt(BlockIDMeta);
            
            return this;
        }
    }
}