
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
    public class LoginSuccessPacket : ProtobufPacket
    {
		public String UUID;
		public String Username;

        public override VarInt ID { get { return 2; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			UUID = reader.Read(UUID);
			Username = reader.Read(Username);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(UUID);
			stream.Write(Username);
          
            return this;
        }

    }
}