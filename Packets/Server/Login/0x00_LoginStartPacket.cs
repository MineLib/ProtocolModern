
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

namespace MineLib.PacketBuilder.Server.Login
{
    public class LoginStartPacket : ProtobufPacket
    {
		public String Name;

        public override VarInt ID { get { return 0; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Name = reader.Read(Name);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Name);
          
            return this;
        }

    }
}