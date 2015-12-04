
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

namespace MineLib.PacketBuilder.Client.Play
{
    public class TeamsPacket : ProtobufPacket
    {
		public String TeamName;
		public SByte Mode;
		public NotSupportedType TeamDisplayName;
		public NotSupportedType TeamPrefix;
		public NotSupportedType TeamSuffix;
		public NotSupportedType FriendlyFire;
		public NotSupportedType NameTagVisibility;
		public NotSupportedType Color;
		public NotSupportedType PlayerCount;
		public NotSupportedType Players;

        public override VarInt ID { get { return 62; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			TeamName = reader.Read(TeamName);
			Mode = reader.Read(Mode);
			TeamDisplayName = reader.Read(TeamDisplayName);
			TeamPrefix = reader.Read(TeamPrefix);
			TeamSuffix = reader.Read(TeamSuffix);
			FriendlyFire = reader.Read(FriendlyFire);
			NameTagVisibility = reader.Read(NameTagVisibility);
			Color = reader.Read(Color);
			Players = reader.Read(Players);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(TeamName);
			stream.Write(Mode);
			stream.Write(TeamDisplayName);
			stream.Write(TeamPrefix);
			stream.Write(TeamSuffix);
			stream.Write(FriendlyFire);
			stream.Write(NameTagVisibility);
			stream.Write(Color);
			stream.Write(Players);
          
            return this;
        }

    }
}