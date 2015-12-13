
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;
using Aragas.Core.IO;

namespace MineLib.PacketBuilder.Client.Play
{
    public class ParticlePacket : ProtobufPacket
    {
		public Int32 ParticleID;
		public Boolean LongDistance;
		public Single X;
		public Single Y;
		public Single Z;
		public Single OffsetX;
		public Single OffsetY;
		public Single OffsetZ;
		public Single ParticleData;
		public Int32 ParticleCount;
		public VarInt[] Data;

        public override VarInt ID { get { return 42; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			ParticleID = reader.Read(ParticleID);
			LongDistance = reader.Read(LongDistance);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			OffsetX = reader.Read(OffsetX);
			OffsetY = reader.Read(OffsetY);
			OffsetZ = reader.Read(OffsetZ);
			ParticleData = reader.Read(ParticleData);
			var DataLength = reader.Read<Int32>();
			Data = reader.Read(Data, DataLength);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(ParticleID);
			stream.Write(LongDistance);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(OffsetX);
			stream.Write(OffsetY);
			stream.Write(OffsetZ);
			stream.Write(ParticleData);
			stream.Write(Data.Length);
			stream.Write(Data);
          
            return this;
        }

    }
}