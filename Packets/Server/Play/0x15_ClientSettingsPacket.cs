
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

namespace MineLib.PacketBuilder.Server.Play
{
    public class ClientSettingsPacket : ProtobufPacket
    {
		public String Locale;
		public SByte ViewDistance;
		public SByte ChatMode;
		public Boolean ChatColors;
		public Byte DisplayedSkinParts;

        public override VarInt ID { get { return 21; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Locale = reader.Read(Locale);
			ViewDistance = reader.Read(ViewDistance);
			ChatMode = reader.Read(ChatMode);
			ChatColors = reader.Read(ChatColors);
			DisplayedSkinParts = reader.Read(DisplayedSkinParts);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Locale);
			stream.Write(ViewDistance);
			stream.Write(ChatMode);
			stream.Write(ChatColors);
			stream.Write(DisplayedSkinParts);
          
            return this;
        }

    }
}