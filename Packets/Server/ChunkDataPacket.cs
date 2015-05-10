using MineLib.Core;
using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.IO;

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
            
            return this;
        }
    }
}