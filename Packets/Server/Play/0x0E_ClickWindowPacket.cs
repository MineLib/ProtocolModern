
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
    public class ClickWindowPacket : ProtobufPacket
    {
		public Byte WindowID;
		public Int16 Slot;
		public SByte Button;
		public Int16 ActionNumber;
		public SByte Mode;
		public ItemStack Clickeditem;

        public override VarInt ID { get { return 14; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			Slot = reader.Read(Slot);
			Button = reader.Read(Button);
			ActionNumber = reader.Read(ActionNumber);
			Mode = reader.Read(Mode);
			Clickeditem = reader.Read(Clickeditem);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(WindowID);
			stream.Write(Slot);
			stream.Write(Button);
			stream.Write(ActionNumber);
			stream.Write(Mode);
			stream.Write(Clickeditem);
          
            return this;
        }

    }
}