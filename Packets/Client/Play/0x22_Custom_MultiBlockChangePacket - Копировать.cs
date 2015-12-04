
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Client.Play
{
    public class MultiBlockChangePacket : ProtobufPacket
    {
        public Int32 ChunkX;
        public Int32 ChunkY;
        public VarInt RecordCount;
        public Record[] Record;


        public override VarInt ID { get { return 0x22; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
            ChunkX = reader.Read(ChunkX);
            ChunkY = reader.Read(ChunkY);
            var RecordLength = reader.Read<VarInt>();
            Record = reader.Read(Record, RecordLength);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
            stream.Write(ChunkX);
            stream.Write(ChunkY);
			stream.Write(Record.Length);
            stream.Write(Record);

            return this;
        }

    }
}