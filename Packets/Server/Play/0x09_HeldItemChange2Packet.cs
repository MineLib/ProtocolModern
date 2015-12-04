
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
    public class HeldItemChange2Packet : ProtobufPacket
    {
		public Int16 Slot;

        public override VarInt ID { get { return 9; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Slot = reader.Read(Slot);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Slot);
          
            return this;
        }

    }
}