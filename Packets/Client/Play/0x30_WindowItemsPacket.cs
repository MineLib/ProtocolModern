
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
    public class WindowItemsPacket : ProtobufPacket
    {
		public Byte WindowID;
		public Int16 Count;
		public ItemStack[] SlotData;

        public override VarInt ID { get { return 48; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			var SlotDataLength = reader.Read<Int16>();
			SlotData = reader.Read(SlotData, SlotDataLength);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(WindowID);
			stream.Write(SlotData.Length);
			stream.Write(SlotData);
          
            return this;
        }

    }
}