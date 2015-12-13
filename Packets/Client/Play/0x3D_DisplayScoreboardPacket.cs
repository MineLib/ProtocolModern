
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

namespace MineLib.PacketBuilder.Client.Play
{
    public class DisplayScoreboardPacket : ProtobufPacket
    {
		public SByte Position;
		public String ScoreName;

        public override VarInt ID { get { return 61; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Position = reader.Read(Position);
			ScoreName = reader.Read(ScoreName);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Position);
			stream.Write(ScoreName);
          
            return this;
        }

    }
}