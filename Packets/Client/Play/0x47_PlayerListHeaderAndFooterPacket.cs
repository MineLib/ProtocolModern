
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
    public class PlayerListHeaderAndFooterPacket : ProtobufPacket
    {
		public String Header;
		public String Footer;

        public override VarInt ID { get { return 71; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Header = reader.Read(Header);
			Footer = reader.Read(Footer);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Header);
			stream.Write(Footer);
          
            return this;
        }

    }
}