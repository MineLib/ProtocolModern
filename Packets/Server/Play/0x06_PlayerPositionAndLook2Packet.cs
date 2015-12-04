
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Server.Play
{
    public class PlayerPositionAndLook2Packet : ProtobufPacket
    {
		public Double X;
		public Double FeetY;
		public Double Z;
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override VarInt ID { get { return 6; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			X = reader.Read(X);
			FeetY = reader.Read(FeetY);
			Z = reader.Read(Z);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(X);
			stream.Write(FeetY);
			stream.Write(Z);
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(OnGround);
          
            return this;
        }

    }
}