
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
    public class SetCompressionPacket : ProtobufPacket
    {
		public VarInt Threshold;

        public override VarInt ID { get { return 70; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Threshold = reader.Read(Threshold);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Threshold);
          
            return this;
        }

    }
}