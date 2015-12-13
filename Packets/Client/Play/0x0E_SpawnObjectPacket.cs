
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
    public class SpawnObjectPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public SByte Type;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public Byte Pitch;
		public Byte Yaw;
		public NotSupportedType Data;

        public override VarInt ID { get { return 14; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Type = reader.Read(Type);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Pitch = reader.Read(Pitch);
			Yaw = reader.Read(Yaw);
			Data = reader.Read(Data);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Type);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Pitch);
			stream.Write(Yaw);
			stream.Write(Data);
          
            return this;
        }

    }
}