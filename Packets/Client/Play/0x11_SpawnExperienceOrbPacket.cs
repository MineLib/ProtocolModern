
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
    public class SpawnExperienceOrbPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Int32 X;
		public Int32 Y;
		public Int32 Z;
		public Int16 Count;

        public override VarInt ID { get { return 17; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			X = reader.Read(X);
			Y = reader.Read(Y);
			Z = reader.Read(Z);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(X);
			stream.Write(Y);
			stream.Write(Z);
          
            return this;
        }

    }
}