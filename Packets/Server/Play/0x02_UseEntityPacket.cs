
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

namespace MineLib.PacketBuilder.Server.Play
{
    public class UseEntityPacket : ProtobufPacket
    {
		public VarInt Target;
		public VarInt Type;
		public NotSupportedType TargetX;
		public NotSupportedType TargetY;
		public NotSupportedType TargetZ;

        public override VarInt ID { get { return 2; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Target = reader.Read(Target);
			Type = reader.Read(Type);
			TargetX = reader.Read(TargetX);
			TargetY = reader.Read(TargetY);
			TargetZ = reader.Read(TargetZ);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Target);
			stream.Write(Type);
			stream.Write(TargetX);
			stream.Write(TargetY);
			stream.Write(TargetZ);
          
            return this;
        }

    }
}