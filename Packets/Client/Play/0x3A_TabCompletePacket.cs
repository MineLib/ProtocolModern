
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
    public class TabCompletePacket : ProtobufPacket
    {
		public VarInt Count;
		public String[] Matches;

        public override VarInt ID { get { return 58; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			var MatchesLength = reader.Read<VarInt>();
			Matches = reader.Read(Matches, MatchesLength);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Matches.Length);
			stream.Write(Matches);
          
            return this;
        }

    }
}