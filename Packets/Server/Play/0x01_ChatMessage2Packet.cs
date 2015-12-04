
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
    public class ChatMessage2Packet : ProtobufPacket
    {
		public String Message;

        public override VarInt ID { get { return 1; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Message = reader.Read(Message);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Message);
          
            return this;
        }

    }
}