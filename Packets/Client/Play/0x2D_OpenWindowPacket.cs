
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;
using ProtocolModern.Extensions;

namespace MineLib.PacketBuilder.Client.Play
{
    public class OpenWindowPacket : ProtobufPacket
    {
		public Byte WindowID;
		public String WindowType;
		public String WindowTitle;
		public Byte NumberOfSlots;
		public NotSupportedType EntityID;

        public override VarInt ID { get { return 45; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			WindowType = reader.Read(WindowType);
			WindowTitle = reader.Read(WindowTitle);
			NumberOfSlots = reader.Read(NumberOfSlots);
			EntityID = reader.Read(EntityID);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(WindowID);
			stream.Write(WindowType);
			stream.Write(WindowTitle);
			stream.Write(NumberOfSlots);
			stream.Write(EntityID);
          
            return this;
        }

    }
}