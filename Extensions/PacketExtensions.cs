using System;
using System.Collections.Generic;

using Aragas.Core.Data;
using Aragas.Core.IO;

using MineLib.Core;
using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;

using static Aragas.Core.IO.PacketDataReader;

namespace ProtocolModern.Extensions
{
    public static class PacketExtensions
    {
        public static void Init()
        {
            ExtendRead<NotSupportedType>(ReadNotSupportedType);
            ExtendRead<Chunk>(ReadChunk);

            ExtendRead<Chunk[]>(ReadChunkArray);
        }

        public static void Write(this PacketStream stream, NotSupportedType value) { }
        private static NotSupportedType ReadNotSupportedType(PacketDataReader reader, int length = 0) { return null; }

        public static void Write(this PacketStream stream, Chunk value) { }
        private static Chunk ReadChunk(PacketDataReader reader, int length = 0)
        {
            var chunk = new Chunk(new Coordinates2D(reader.Read<int>(), reader.Read<int>()));
            //chunk.Coordinates = new Coordinates2D(reader.Read<int>(), reader.Read<int>());
            chunk.GroundUp = reader.Read<bool>();
            chunk.PrimaryBitMap = reader.Read<ushort>();
            chunk.OverWorld = true; // TODO: From World class

            var size = reader.Read<VarInt>();
            var data = reader.Read<byte[]>(null, size);

            var sectionCount = Chunk.GetSectionCount(chunk.PrimaryBitMap);

            var chunkRawBlocks      = new byte[sectionCount * Chunk.TwoByteData];
            var chunkRawBlocksLight = new byte[sectionCount * Chunk.HalfByteData];
            var chunkRawSkylight    = new byte[sectionCount * Chunk.HalfByteData];

            Buffer.BlockCopy(data, 0,                                                     chunkRawBlocks,         0, chunkRawBlocks.Length * sizeof(byte)       );
            Buffer.BlockCopy(data, chunkRawBlocks.Length,                                 chunkRawBlocksLight,    0, chunkRawBlocksLight.Length * sizeof(byte)  );
            Buffer.BlockCopy(data, chunkRawBlocks.Length + chunkRawBlocksLight.Length,    chunkRawSkylight,       0, chunkRawSkylight.Length * sizeof(byte)     );

            for (int y = 0, i = 0; y < 16; y++)
            {
                if ((chunk.PrimaryBitMap & (1 << y)) > 0)
                {
                    // Blocks & Metadata
                    var rawBlocks = new byte[Chunk.TwoByteData];
                    Buffer.BlockCopy(chunkRawBlocks, i * rawBlocks.Length, rawBlocks, 0, rawBlocks.Length * sizeof(byte));

                    // Light, convert to 1 byte per block
                    var rawBlockLight = new byte[Chunk.HalfByteData];
                    Buffer.BlockCopy(chunkRawSkylight, i * rawBlockLight.Length, rawBlockLight, 0, rawBlockLight.Length * sizeof(byte));

                    // Sky light, convert to 1 byte per block
                    var rawSkyLight = new byte[Chunk.HalfByteData];
                    if (chunk.OverWorld)
                        Buffer.BlockCopy(chunkRawSkylight, i * rawSkyLight.Length, rawSkyLight, 0, rawSkyLight.Length * sizeof(byte));

                    chunk.Sections[y].BuildFromNibbleData(rawBlocks, rawBlockLight, rawSkyLight);
                    i++;
                }
            }
            if (chunk.GroundUp)
                Buffer.BlockCopy(data, data.Length - chunk.Biomes.Length, chunk.Biomes, 0, chunk.Biomes.Length * sizeof(byte));

            return chunk;
        }

        public static void Write(this PacketStream stream, Chunk[] chunkList) { }
        private static Chunk[] ReadChunkArray(PacketDataReader reader, int length = 0)
        {
            var groundUp = reader.Read<bool>();

            var count = reader.Read<VarInt>();
            var metadata = reader.Read<ChunkColumnMetadata[]>(null, count);
            //var metadata = ChunkColumnMetadataList.FromReader(reader);

            int totalSections = 0;
            foreach (var meta in metadata)
                totalSections += Chunk.GetSectionCount(meta.PrimaryBitMap);


            var size = totalSections * (Chunk.TwoByteData + Chunk.HalfByteData + (groundUp ? Chunk.HalfByteData : 0)) + metadata.Length * Chunk.BiomesLength;
            var data = reader.Read<byte[]>(null, size);

            var chunks = new List<Chunk>();
            int offset = 0;
            foreach (var meta in metadata)
            {
                var chunk = new Chunk(meta.Coordinates);
                chunk.OverWorld = true;
                chunk.PrimaryBitMap = meta.PrimaryBitMap;

                var sectionCount = Chunk.GetSectionCount(chunk.PrimaryBitMap);

                var chunkRawBlocks = new byte[sectionCount * Chunk.TwoByteData];
                var chunkRawBlocksLight = new byte[sectionCount * Chunk.HalfByteData];
                var chunkRawSkylight = new byte[sectionCount * Chunk.HalfByteData];

                var chunkLength = sectionCount * (Chunk.TwoByteData + Chunk.HalfByteData + (chunk.OverWorld ? Chunk.HalfByteData : 0)) + Chunk.BiomesLength;
                var chunkData = new byte[chunkLength];
                Buffer.BlockCopy(data, offset, chunkData, 0, chunkData.Length * sizeof(byte));

                Buffer.BlockCopy(chunkData, 0, chunkRawBlocks, 0, chunkRawBlocks.Length * sizeof(byte));
                Buffer.BlockCopy(chunkData, chunkRawBlocks.Length, chunkRawBlocksLight, 0, chunkRawBlocksLight.Length * sizeof(byte));
                Buffer.BlockCopy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length, chunkRawSkylight, 0, chunkRawSkylight.Length * sizeof(byte));
                if (groundUp)
                    Buffer.BlockCopy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length + chunkRawSkylight.Length, chunk.Biomes, 0, Chunk.BiomesLength * sizeof(byte));

                for (int y = 0, i = 0; y < 16; y++)
                {
                    if ((chunk.PrimaryBitMap & (1 << y)) > 0)
                    {
                        // Blocks & Metadata
                        var rawBlocks = new byte[Chunk.TwoByteData];
                        Buffer.BlockCopy(chunkRawBlocks, i * rawBlocks.Length, rawBlocks, 0, rawBlocks.Length * sizeof(byte));

                        // Light
                        var rawBlockLight = new byte[Chunk.HalfByteData];
                        Buffer.BlockCopy(chunkRawSkylight, i * rawBlockLight.Length, rawBlockLight, 0, rawBlockLight.Length * sizeof(byte));

                        // Sky light
                        var rawSkyLight = new byte[Chunk.HalfByteData];
                        if (chunk.OverWorld)
                            Buffer.BlockCopy(chunkRawSkylight, i * rawSkyLight.Length, rawSkyLight, 0, rawSkyLight.Length * sizeof(byte));

                        chunk.Sections[y].BuildFromNibbleData(rawBlocks, rawBlockLight, rawSkyLight);
                        i++;
                    }
                }
                chunks.Add(chunk);

                offset += chunkLength;
            }

            if (offset != data.Length)
                throw new NetworkHandlerException("Map Chunk Bulk reading error: offset != data.Length");

            return chunks.ToArray();
        }
    }
}
