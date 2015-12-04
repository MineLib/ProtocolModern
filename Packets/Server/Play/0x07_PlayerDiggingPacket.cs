
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Server.Play
{
    public class PlayerDiggingPacket : ProtobufPacket
    {
		public SByte Status;
		public Position Location;
		public SByte Face;

        public override VarInt ID { get { return 7; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Status = reader.Read(Status);
			Location = reader.Read(Location);
			Face = reader.Read(Face);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Status);
			stream.Write(Location);
			stream.Write(Face);
          
            return this;
        }

    }
}