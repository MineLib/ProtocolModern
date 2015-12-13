
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
    public class ClientStatusPacket : ProtobufPacket
    {
		public VarInt ActionID;

        public override VarInt ID { get { return 22; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			ActionID = reader.Read(ActionID);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(ActionID);
          
            return this;
        }

    }
}