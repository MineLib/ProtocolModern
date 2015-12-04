
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
    public class TimeUpdatePacket : ProtobufPacket
    {
		public Int64 WorldAge;
		public Int64 Timeofday;

        public override VarInt ID { get { return 3; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			WorldAge = reader.Read(WorldAge);
			Timeofday = reader.Read(Timeofday);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(WorldAge);
			stream.Write(Timeofday);
          
            return this;
        }

    }
}