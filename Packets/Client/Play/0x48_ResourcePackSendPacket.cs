
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Client.Play
{
    public class ResourcePackSendPacket : ProtobufPacket
    {
		public String URL;
		public String Hash;

        public override VarInt ID { get { return 72; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			URL = reader.Read(URL);
			Hash = reader.Read(Hash);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(URL);
			stream.Write(Hash);
          
            return this;
        }

    }
}