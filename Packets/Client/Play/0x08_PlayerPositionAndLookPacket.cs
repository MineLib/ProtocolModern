
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
    public class PlayerPositionAndLookPacket : ProtobufPacket
    {
		public Double X;
		public Double Y;
		public Double Z;
		public Single Yaw;
		public Single Pitch;
		public SByte Flags;

        public override VarInt ID { get { return 8; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			Flags = reader.Read(Flags);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(Flags);
          
            return this;
        }

    }
}