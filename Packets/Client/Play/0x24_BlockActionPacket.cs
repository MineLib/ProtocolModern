
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
    public class BlockActionPacket : ProtobufPacket
    {
		public Position Location;
		public Byte Byte1;
		public Byte Byte2;
		public VarInt BlockType;

        public override VarInt ID { get { return 36; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Location = reader.Read(Location);
			Byte1 = reader.Read(Byte1);
			Byte2 = reader.Read(Byte2);
			BlockType = reader.Read(BlockType);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Location);
			stream.Write(Byte1);
			stream.Write(Byte2);
			stream.Write(BlockType);
          
            return this;
        }

    }
}