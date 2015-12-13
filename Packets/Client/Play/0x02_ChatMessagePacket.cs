
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
    public class ChatMessagePacket : ProtobufPacket
    {
		public String JSONData;
		public SByte Position;

        public override VarInt ID { get { return 2; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			JSONData = reader.Read(JSONData);
			Position = reader.Read(Position);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(JSONData);
			stream.Write(Position);
          
            return this;
        }

    }
}