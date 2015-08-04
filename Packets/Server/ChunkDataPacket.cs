using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Extensions;

namespace ProtocolModern.Packets.Server
{
    public struct ChunkDataPacket : IPacket
    {
        public Chunk Chunk { get; set; }

        public byte ID { get { return 0x21; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Chunk = new Chunk(Coordinates2D.Zero).FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            Chunk.ToStream(stream);
            
            return this;
        }
    }
}