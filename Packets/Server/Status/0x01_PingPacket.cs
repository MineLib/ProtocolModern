
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

namespace MineLib.PacketBuilder.Server.Status
{
    public class PingPacket : ProtobufPacket
    {
		public Int64 Payload;

        public override VarInt ID { get { return 1; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Payload = reader.Read(Payload);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Payload);
          
            return this;
        }

    }
}