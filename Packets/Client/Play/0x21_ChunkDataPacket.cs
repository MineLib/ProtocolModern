
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
    public class ChunkDataPacket : ProtobufPacket
    {
		public Int32 ChunkX;
		public Int32 ChunkZ;
		public Boolean GroundUpContinuous;
		public UInt16 PrimaryBitMask;
		public VarInt Size;
		public Chunk Data;

        public override VarInt ID { get { return 33; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			ChunkX = reader.Read(ChunkX);
			ChunkZ = reader.Read(ChunkZ);
			GroundUpContinuous = reader.Read(GroundUpContinuous);
			PrimaryBitMask = reader.Read(PrimaryBitMask);
			Size = reader.Read(Size);
			Data = reader.Read(Data);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(ChunkX);
			stream.Write(ChunkZ);
			stream.Write(GroundUpContinuous);
			stream.Write(PrimaryBitMask);
			stream.Write(Size);
			stream.Write(Data);
          
            return this;
        }

    }
}