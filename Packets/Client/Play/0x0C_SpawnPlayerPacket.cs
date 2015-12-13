
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
using ProtocolModern.Extensions;

namespace MineLib.PacketBuilder.Client.Play
{
    public class SpawnPlayerPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public NotSupportedType PlayerUUID;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public Byte Yaw;
		public Byte Pitch;
		public Int16 CurrentItem;
		public EntityMetadataList Metadata;

        public override VarInt ID { get { return 12; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			PlayerUUID = reader.Read(PlayerUUID);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			CurrentItem = reader.Read(CurrentItem);
			Metadata = reader.Read(Metadata);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(PlayerUUID);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(CurrentItem);
			stream.Write(Metadata);
          
            return this;
        }

    }
}