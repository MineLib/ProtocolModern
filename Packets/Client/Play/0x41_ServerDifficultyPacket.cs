
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
    public class ServerDifficultyPacket : ProtobufPacket
    {
		public Byte Difficulty;

        public override VarInt ID { get { return 65; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Difficulty = reader.Read(Difficulty);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Difficulty);
          
            return this;
        }

    }
}