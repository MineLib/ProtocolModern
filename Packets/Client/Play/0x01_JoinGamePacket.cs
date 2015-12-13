
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
    public class JoinGamePacket : ProtobufPacket
    {
		public Int32 EntityID;
		public Byte Gamemode;
		public SByte Dimension;
		public Byte Difficulty;
		public Byte MaxPlayers;
		public String LevelType;
		public Boolean ReducedDebugInfo;

        public override VarInt ID { get { return 1; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Gamemode = reader.Read(Gamemode);
			Dimension = reader.Read(Dimension);
			Difficulty = reader.Read(Difficulty);
			MaxPlayers = reader.Read(MaxPlayers);
			LevelType = reader.Read(LevelType);
			ReducedDebugInfo = reader.Read(ReducedDebugInfo);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Gamemode);
			stream.Write(Dimension);
			stream.Write(Difficulty);
			stream.Write(MaxPlayers);
			stream.Write(LevelType);
			stream.Write(ReducedDebugInfo);
          
            return this;
        }

    }
}