
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
    public class SpawnMobPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Byte Type;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public Byte Yaw;
		public Byte Pitch;
		public Byte HeadPitch;
		public Int16 VelocityX;
		public Int16 VelocityY;
		public Int16 VelocityZ;
		public EntityMetadataList Metadata;

        public override VarInt ID { get { return 15; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Type = reader.Read(Type);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			HeadPitch = reader.Read(HeadPitch);
			VelocityX = reader.Read(VelocityX);
			VelocityY = reader.Read(VelocityY);
			VelocityZ = reader.Read(VelocityZ);
			Metadata = reader.Read(Metadata);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Type);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(HeadPitch);
			stream.Write(VelocityX);
			stream.Write(VelocityY);
			stream.Write(VelocityZ);
			stream.Write(Metadata);
          
            return this;
        }

    }
}