
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
    public class OpenSignEditorPacket : ProtobufPacket
    {
		public Position Location;

        public override VarInt ID { get { return 54; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Location = reader.Read(Location);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Location);
          
            return this;
        }

    }
}