
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
    public class EntityLookAndRelativeMovePacket : ProtobufPacket
    {
		public VarInt EntityID;
		public SByte DeltaX;
		public SByte DeltaY;
		public SByte DeltaZ;
		public Byte Yaw;
		public Byte Pitch;
		public Boolean OnGround;

        public override VarInt ID { get { return 23; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			DeltaX = reader.Read(DeltaX);
			DeltaY = reader.Read(DeltaY);
			DeltaZ = reader.Read(DeltaZ);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(DeltaX);
			stream.Write(DeltaY);
			stream.Write(DeltaZ);
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(OnGround);
          
            return this;
        }

    }
}