
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
    public class EntityVelocityPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Int16 VelocityX;
		public Int16 VelocityY;
		public Int16 VelocityZ;

        public override VarInt ID { get { return 18; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			VelocityX = reader.Read(VelocityX);
			VelocityY = reader.Read(VelocityY);
			VelocityZ = reader.Read(VelocityZ);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(VelocityX);
			stream.Write(VelocityY);
			stream.Write(VelocityZ);
          
            return this;
        }

    }
}