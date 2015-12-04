
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
    public class WindowPropertyPacket : ProtobufPacket
    {
		public Byte WindowID;
		public Int16 Property;
		public Int16 Value;

        public override VarInt ID { get { return 49; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			Property = reader.Read(Property);
			Value = reader.Read(Value);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(WindowID);
			stream.Write(Property);
			stream.Write(Value);
          
            return this;
        }

    }
}