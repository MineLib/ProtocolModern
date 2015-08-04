using MineLib.Core;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;
using MineLib.Core.Wrappers;
using ProtocolModern.Extensions;

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