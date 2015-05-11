using MineLib.Core;
using MineLib.Core.Data.Anvil;
using MineLib.Core.IO;

using ProtocolModern.Data.Anvil;

namespace ProtocolModern.Packets.Server
{
    public struct MapChunkBulkPacket : IPacket
    {
        public ChunkList ChunkList;

        public byte ID { get { return 0x26; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            ChunkList = new ChunkList().FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            ChunkList.ToStream(stream);
            
            return this;
        }
    }
}