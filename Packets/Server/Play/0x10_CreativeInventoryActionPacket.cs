
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

namespace MineLib.PacketBuilder.Server.Play
{
    public class CreativeInventoryActionPacket : ProtobufPacket
    {
		public Int16 Slot;
		public ItemStack ClickedItem;

        public override VarInt ID { get { return 16; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Slot = reader.Read(Slot);
			ClickedItem = reader.Read(ClickedItem);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Slot);
			stream.Write(ClickedItem);
          
            return this;
        }

    }
}