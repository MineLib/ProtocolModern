
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
    public class EntityRelativeMovePacket : ProtobufPacket
    {
		public VarInt EntityID;
		public SByte DeltaX;
		public SByte DeltaY;
		public SByte DeltaZ;
		public Boolean OnGround;

        public override VarInt ID { get { return 21; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			DeltaX = reader.Read(DeltaX);
			DeltaY = reader.Read(DeltaY);
			DeltaZ = reader.Read(DeltaZ);
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(DeltaX);
			stream.Write(DeltaY);
			stream.Write(DeltaZ);
			stream.Write(OnGround);
          
            return this;
        }

    }
}