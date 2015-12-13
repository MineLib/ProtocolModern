
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

namespace MineLib.PacketBuilder.Server.Play
{
    public class PlayerAbilities2Packet : ProtobufPacket
    {
		public SByte Flags;
		public Single FlyingSpeed;
		public Single WalkingSpeed;

        public override VarInt ID { get { return 19; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Flags = reader.Read(Flags);
			FlyingSpeed = reader.Read(FlyingSpeed);
			WalkingSpeed = reader.Read(WalkingSpeed);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Flags);
			stream.Write(FlyingSpeed);
			stream.Write(WalkingSpeed);
          
            return this;
        }

    }
}