
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
    public class PlayerLookPacket : ProtobufPacket
    {
		public Single Yaw;
		public Single Pitch;
		public Boolean OnGround;

        public override VarInt ID { get { return 5; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Yaw = reader.Read(Yaw);
			Pitch = reader.Read(Pitch);
			OnGround = reader.Read(OnGround);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Yaw);
			stream.Write(Pitch);
			stream.Write(OnGround);
          
            return this;
        }

    }
}