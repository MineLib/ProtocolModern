
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
    public class ConfirmTransaction2Packet : ProtobufPacket
    {
		public SByte WindowID;
		public Int16 ActionNumber;
		public Boolean Accepted;

        public override VarInt ID { get { return 15; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			ActionNumber = reader.Read(ActionNumber);
			Accepted = reader.Read(Accepted);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(WindowID);
			stream.Write(ActionNumber);
			stream.Write(Accepted);
          
            return this;
        }

    }
}