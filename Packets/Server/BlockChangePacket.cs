using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct BlockChangePacket : IPacket
    {
        public Position Location;
        public int BlockIDMeta;

        public byte ID { get { return 0x23; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromLong(reader.ReadLong());
            BlockIDMeta = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            stream.WriteVarInt(BlockIDMeta);
            stream.Purge();

            return this;
        }
    }
}