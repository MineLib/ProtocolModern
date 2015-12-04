
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;
using ProtocolModern.Extensions;

namespace MineLib.PacketBuilder.Client.Play
{
    public class MapChunkBulkPacket : ProtobufPacket
    {
        public Boolean SkyLightSent;
        public VarInt ChunkColumnCount;
        public ChunkColumnMetadata[] ChunkMeta;
        public Chunk[] ChunkData;


        public override VarInt ID { get { return 0x22; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
            //SkyLightSent = reader.Read(SkyLightSent);
            //var ChunkMetaLength = reader.Read<VarInt>();
            //ChunkMeta = reader.Read(ChunkMeta, ChunkMetaLength);
            ChunkData = reader.Read(ChunkData);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
            stream.Write(SkyLightSent);
            stream.Write(ChunkMeta.Length);
			stream.Write(ChunkMeta);
            stream.Write(ChunkData);

            return this;
        }

    }
}