
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
    public class BlockChangePacket : ProtobufPacket
    {
		public Position Location;
		public VarInt BlockID;

        public override VarInt ID { get { return 35; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
            Location = reader.Read(Location);
			BlockID = reader.Read(BlockID);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Location);
			stream.Write(BlockID);
          
            return this;
        }

    }
}