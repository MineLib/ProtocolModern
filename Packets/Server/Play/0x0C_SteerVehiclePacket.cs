
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
    public class SteerVehiclePacket : ProtobufPacket
    {
		public Single Sideways;
		public Single Forward;
		public Byte Flags;

        public override VarInt ID { get { return 12; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Sideways = reader.Read(Sideways);
			Forward = reader.Read(Forward);
			Flags = reader.Read(Flags);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Sideways);
			stream.Write(Forward);
			stream.Write(Flags);
          
            return this;
        }

    }
}