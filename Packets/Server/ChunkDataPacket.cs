using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Anvil;
using MineLib.Network.IO;

using ProtocolModern.Data.Anvil;

namespace ProtocolModern.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Chunk Chunk;

        public byte ID { get { return 0x21; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Chunk = new Chunk(Coordinates2D.Zero).FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Chunk.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}