
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
    public class RespawnPacket : ProtobufPacket
    {
		public Int32 Dimension;
		public Byte Difficulty;
		public Byte Gamemode;
		public String LevelType;

        public override VarInt ID { get { return 7; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Dimension = reader.Read(Dimension);
			Difficulty = reader.Read(Difficulty);
			Gamemode = reader.Read(Gamemode);
			LevelType = reader.Read(LevelType);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Dimension);
			stream.Write(Difficulty);
			stream.Write(Gamemode);
			stream.Write(LevelType);
          
            return this;
        }

    }
}