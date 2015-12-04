
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
    public class ExplosionPacket : ProtobufPacket
    {
		public Single X;
		public Single Y;
		public Single Z;
		public Single Radius;
		public Int32 RecordCount;
		public Byte[] Records;
		public Single PlayerMotionX;
		public Single PlayerMotionY;
		public Single PlayerMotionZ;

        public override VarInt ID { get { return 39; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);
			Radius = reader.Read(Radius);
			var RecordsLength = reader.Read<Int32>();
			Records = reader.Read(Records, RecordsLength);
			PlayerMotionX = reader.Read(PlayerMotionX);
			PlayerMotionY = reader.Read(PlayerMotionY);
			PlayerMotionZ = reader.Read(PlayerMotionZ);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
			stream.Write(Radius);
			stream.Write(Records.Length);
			stream.Write(Records);
			stream.Write(PlayerMotionX);
			stream.Write(PlayerMotionY);
			stream.Write(PlayerMotionZ);
          
            return this;
        }

    }
}