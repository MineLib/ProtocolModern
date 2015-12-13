
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
    public class ScoreboardObjectivePacket : ProtobufPacket
    {
		public String ObjectiveName;
		public SByte Mode;
		public NotSupportedType ObjectiveValue;
		public NotSupportedType Type;

        public override VarInt ID { get { return 59; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			ObjectiveName = reader.Read(ObjectiveName);
			Mode = reader.Read(Mode);
			ObjectiveValue = reader.Read(ObjectiveValue);
			Type = reader.Read(Type);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(ObjectiveName);
			stream.Write(Mode);
			stream.Write(ObjectiveValue);
			stream.Write(Type);
          
            return this;
        }

    }
}