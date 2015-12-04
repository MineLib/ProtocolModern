
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

namespace MineLib.PacketBuilder.Server.Play
{
    public class TabComplete2Packet : ProtobufPacket
    {
		public String Text;
		public Boolean HasPosition;
		public NotSupportedType LookedAtBlock;

        public override VarInt ID { get { return 20; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Text = reader.Read(Text);
			HasPosition = reader.Read(HasPosition);
			LookedAtBlock = reader.Read(LookedAtBlock);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Text);
			stream.Write(HasPosition);
			stream.Write(LookedAtBlock);
          
            return this;
        }

    }
}