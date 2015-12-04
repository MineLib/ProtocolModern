
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
    public class SetSlotPacket : ProtobufPacket
    {
		public SByte WindowID;
		public Int16 Slot;
		public ItemStack SlotData;

        public override VarInt ID { get { return 47; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			Slot = reader.Read(Slot);
			SlotData = reader.Read(SlotData);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(WindowID);
			stream.Write(Slot);
			stream.Write(SlotData);
          
            return this;
        }

    }
}