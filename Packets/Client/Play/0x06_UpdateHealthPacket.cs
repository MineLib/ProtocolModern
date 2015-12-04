
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
    public class UpdateHealthPacket : ProtobufPacket
    {
		public Single Health;
		public VarInt Food;
		public Single FoodSaturation;

        public override VarInt ID { get { return 6; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Health = reader.Read(Health);
			Food = reader.Read(Food);
			FoodSaturation = reader.Read(FoodSaturation);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Health);
			stream.Write(Food);
			stream.Write(FoodSaturation);
          
            return this;
        }

    }
}