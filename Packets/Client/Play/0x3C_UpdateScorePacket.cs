
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
using ProtocolModern.Extensions;

namespace MineLib.PacketBuilder.Client.Play
{
    public class UpdateScorePacket : ProtobufPacket
    {
		public String ScoreName;
		public SByte Action;
		public String ObjectiveName;
		public NotSupportedType Value;

        public override VarInt ID { get { return 60; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			ScoreName = reader.Read(ScoreName);
			Action = reader.Read(Action);
			ObjectiveName = reader.Read(ObjectiveName);
			Value = reader.Read(Value);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(ScoreName);
			stream.Write(Action);
			stream.Write(ObjectiveName);
			stream.Write(Value);
          
            return this;
        }

    }
}