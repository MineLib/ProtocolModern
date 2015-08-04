//#define PARALLEL

using System;
using System.Collections.Generic;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Extensions
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

        public static void ToStream(this Chunk chunk, IProtocolStream stream) { }
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
#if PARALLEL
            var overWorld = true;
            List<int> chunkLen = new List<int>();
            var metadat = new List<ChunkColumnMetadata>(metadata.GetMetadata());
            foreach (var meta in metadat)
            {
                var sectionCount = Chunk.GetSectionCount(meta.PrimaryBitMap);
                chunkLen.Add(sectionCount * (Chunk.TwoByteData + Chunk.HalfByteData + (overWorld ? Chunk.HalfByteData : 0)) + Chunk.BiomesLength);
            }

            ParallelOptions op = new ParallelOptions();
            op.MaxDegreeOfParallelism = 2;
            Parallel.For(0, metadat.Count, op, delegate(int k)
            {
                int offsetT = 0;
                for (int i = 0; i < k; i++)
                    offsetT += chunkLen[i];

                var meta = metadat[k];

                var chunk = new Chunk(meta.Coordinates);
                chunk.OverWorld = true;
                chunk.PrimaryBitMap = meta.PrimaryBitMap;

                var sectionCount = Chunk.GetSectionCount(chunk.PrimaryBitMap);

                var chunkRawBlocks = new byte[sectionCount*Chunk.TwoByteData];
                var chunkRawBlocksLight = new byte[sectionCount*Chunk.HalfByteData];
                var chunkRawSkylight = new byte[sectionCount*Chunk.HalfByteData];

                var chunkLength = sectionCount*
                                  (Chunk.TwoByteData + Chunk.HalfByteData + (chunk.OverWorld ? Chunk.HalfByteData : 0)) +
                                  Chunk.BiomesLength;
                var chunkData = new byte[chunkLength];
                Buffer.BlockCopy(data, offsetT, chunkData, 0, chunkData.Length*sizeof (byte));

                Buffer.BlockCopy(chunkData, 0, chunkRawBlocks, 0, chunkRawBlocks.Length*sizeof (byte));
                Buffer.BlockCopy(chunkData, chunkRawBlocks.Length, chunkRawBlocksLight, 0,
                    chunkRawBlocksLight.Length*sizeof (byte));
                Buffer.BlockCopy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length, chunkRawSkylight, 0,
                    chunkRawSkylight.Length*sizeof (byte));
                if (groundUp)
                    Buffer.BlockCopy(chunkData,
                        chunkRawBlocks.Length + chunkRawBlocksLight.Length + chunkRawSkylight.Length, chunk.Biomes, 0,
                        Chunk.BiomesLength*sizeof (byte));

                for (int y = 0, i = 0; y < 16; y++)
                {
                    if ((chunk.PrimaryBitMap & (1 << y)) > 0)
                    {
                        // Blocks & Metadata
                        var rawBlocks = new byte[Chunk.TwoByteData];
                        Buffer.BlockCopy(chunkRawBlocks, i*rawBlocks.Length, rawBlocks, 0,
                            rawBlocks.Length*sizeof (byte));

                        // Light
                        var rawBlockLight = new byte[Chunk.HalfByteData];
                        Buffer.BlockCopy(chunkRawSkylight, i*rawBlockLight.Length, rawBlockLight, 0,
                            rawBlockLight.Length*sizeof (byte));

                        // Sky light
                        var rawSkyLight = new byte[Chunk.HalfByteData];
                        if (chunk.OverWorld)
                            Buffer.BlockCopy(chunkRawSkylight, i*rawSkyLight.Length, rawSkyLight, 0,
                                rawSkyLight.Length*sizeof (byte));

                        chunk.Sections[y].BuildFromNibbleData(rawBlocks, rawBlockLight, rawSkyLight);
                        i++;
                    }
                }
                chunks.Add(chunk);
            });   
 
            int offset = 0;
            foreach (var i in chunkLen)
                offset += i;
#else
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
                Buffer.BlockCopy(data, offset, chunkData, 0, chunkData.Length * sizeof(byte));

                Buffer.BlockCopy(chunkData, 0,                                                    chunkRawBlocks,         0, chunkRawBlocks.Length * sizeof(byte)       );
                Buffer.BlockCopy(chunkData, chunkRawBlocks.Length,                                chunkRawBlocksLight,    0, chunkRawBlocksLight.Length * sizeof(byte)  );
                Buffer.BlockCopy(chunkData, chunkRawBlocks.Length + chunkRawBlocksLight.Length,   chunkRawSkylight,       0, chunkRawSkylight.Length * sizeof(byte)     );
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
#endif
            
            if (offset != data.Length)
                throw new NetworkHandlerException("Map Chunk Bulk reading error: offset != data.Length");

            chunkList = new ChunkList(chunks) { GroundUp = groundUp, Metadata = metadata };

            return chunkList;
        }

        public static void ToStream(this ChunkList chunkList, IProtocolStream stream) { }
    }
}
