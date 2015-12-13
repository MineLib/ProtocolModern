
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

namespace MineLib.PacketBuilder.Client.Login
{
    public class Disconnect2Packet : ProtobufPacket
    {
		public String Reason;

        public override VarInt ID { get { return 0; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Reason = reader.Read(Reason);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Reason);
          
            return this;
        }

    }
}