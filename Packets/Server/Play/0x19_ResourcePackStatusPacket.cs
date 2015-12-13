
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
    public class ResourcePackStatusPacket : ProtobufPacket
    {
		public String Hash;
		public VarInt Result;

        public override VarInt ID { get { return 25; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Hash = reader.Read(Hash);
			Result = reader.Read(Result);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Hash);
			stream.Write(Result);
          
            return this;
        }

    }
}