using System;
using System.Collections.Generic;

using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Anvil;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;

namespace ProtocolModern.Data.Anvil
{
    public static class ChunkExtention
    {
        public static Chunk FromReader(this Chunk chunk, IProtocolDataReader reader)
        {
            chunk.OverWorld = true; // TODO: From World class
            chunk.Coordinates = new Coordinates2D(reader.ReadInt(), reader.ReadInt());
            chunk.GroundUp = reader.ReadBoolean();
            chunk.PrimaryBitMap = reader.ReadUShort();

            var size = reader.ReadVarInt();
            var data = reader.ReadByteArray(size);

            var sectionCount = Chunk.GetSectionCount(chunk.PrimaryBitMap);

            var chunkRawBlocks      = new byte[sectionCount * Chunk.TwoByteData];
            var chunkRawBlocksLight = new byte[sectionCount * Chunk.HalfByteData];
            var chunkRawSkylight    = new byte[sectionCount * Chunk.HalfByteData];

            Array.Copy(data, 0,                                                     chunkRawBlocks,         0, chunkRawBlocks.Length        );
            Array.Copy(data, chunkRawBlocks.Length,                                 chunkRawBlocksLight,    0, chunkRawBlocksLight.Length   );
            Array.Copy(data, chunkRawBlocks.Length + chunkRawBlocksLight.Length,    chunkRawSkylight,       0, chunkRawSkylight.Length      );

            for (int y = 0, i = 0; y < 16; y++)
            {
                if ((chunk.PrimaryBitMap & (1 << y)) > 0)
                {
                    // Blocks & Metadata
                    var rawBlocks = new byte[Chunk.TwoByteData];
                    Array.Copy(chunkRawBlocks, i * rawBlocks.Length, rawBlocks, 0, rawBlocks.Length);

                    // Light, convert to 1 byte per block
                    var rawBlockLight = new byte[Chunk.HalfByteData];
                    Array.Copy(chunkRawSkylight, i * rawBlockLight.Length, rawBlockLight, 0, rawBlockLight.Length);

                    // Sky light, convert to 1 byte per block
                    var rawSkyLight = new byte[Chunk.HalfByteData];
                    if (chunk.OverWorld)
                        Array.Copy(chunkRawSkylight, i * rawSkyLight.Length, rawSkyLight, 0, rawSkyLight.Length);

                    chunk.Sections[y].BuildFromNibbleData(rawBlocks, rawBlockLight, rawSkyLight);
                    i++;
                }
            }
            if (chunk.GroundUp)
                Array.Copy(data, data.Length - chunk.Biomes.Length, chunk.Biomes, 0, chunk.Biomes.Length);

            return chunk;
        }

        public static void ToStream(this Chunk chunk, IProtocolStream stream)
        {
        }
    }

    public static class ChunkListExtention
    {
        public static ChunkList FromReader(this ChunkList chunkList, IProtocolDataReader reader)
        {
            var groundUp = reader.ReadBoolean();
            var metadata = ChunkColumnMetadataList.FromReader(reader);

            int totalSections = 0;
            foreach (var meta in metadata.GetMetadata())
                totalSections += Chunk.GetSectionCount(meta.PrimaryBitMap);


            var size = totalSections * (Chunk.TwoByteData + Chunk.HalfByteData + (groundUp ? Chunk.HalfByteData : 0)) + metadata.Count * Chunk.BiomesLength;
            var data = reader.ReadByteArray(size);

            var chunks = new List<Chunk>();
            int offset = 0;
            foreach (var meta in metadata.GetMetadata())
            {
                var chunk = new Chunk(meta.Coordinates);
                chunk.OverWorld = true;
                chunk.PrimaryBitMap = meta.PrimaryBitMap;

                var sectionCount = Chunk.GetSectionCount(chunk.PrimaryBitMap);

                var chunkRawBlocks      = new byte[sectionCount * Chunk.TwoByteData];
                var chunkRawBlocksLight = new byte[sectionCount * Chunk.HalfByteData];
                var chunkRawSkylight    = new byte[sectionCount * Chunk.HalfByteData];

                var chunkLength = sectionCount * (Chunk.TwoByteData + Chunk.HalfByteData + (chunk.OverWorld ? Chunk.HalfByteData : 0)) + Chunk.BiomesLength;
                var chunkData = new byte[chunkLength];
                Array.Copy(data, offset, chunkData, 0, chunkData.Length);

                Array.Copy(chunkData, 0,                                                    chunkRawBlocks,         0, chunkRawBlocks.Length        );
                Array.Copy(chunkData, chunkRawBlocks.Length,                                chunkRawBlocksLight,    0, chunkRawBlocksLight.Length   );
                Array.Copy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length,   chunkRawSkylight,       0, chunkRawSkylight.Length      );
                if (groundUp)
                    Array.Copy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length + chunkRawSkylight.Length, chunk.Biomes, 0, Chunk.BiomesLength);

                for (int y = 0, i = 0; y < 16; y++)
                {
                    if ((chunk.PrimaryBitMap & (1 << y)) > 0)
                    {
                        // Blocks & Metadata
                        var rawBlocks = new byte[Chunk.TwoByteData];
                        Array.Copy(chunkRawBlocks, i * rawBlocks.Length, rawBlocks, 0, rawBlocks.Length);

                        // Light
                        var rawBlockLight = new byte[Chunk.HalfByteData];
                        Array.Copy(chunkRawSkylight, i * rawBlockLight.Length, rawBlockLight, 0, rawBlockLight.Length);

                        // Sky light
                        var rawSkyLight = new byte[Chunk.HalfByteData];
                        if (chunk.OverWorld)
                            Array.Copy(chunkRawSkylight, i * rawSkyLight.Length, rawSkyLight, 0, rawSkyLight.Length);

                        chunk.Sections[y].BuildFromNibbleData(rawBlocks, rawBlockLight, rawSkyLight);
                        i++;
                    }
                }
                chunks.Add(chunk);

                offset += chunkLength;
            }

            if (offset != data.Length)
                throw new NetworkHandlerException("Map Chunk Bulk reading error: offset != data.Length");

            chunkList = new ChunkList(chunks) { GroundUp = groundUp, Metadata = metadata };

            return chunkList;
        }

        public static void ToStream(this ChunkList chunkList, IProtocolStream stream)
        {

        }
    }
}
