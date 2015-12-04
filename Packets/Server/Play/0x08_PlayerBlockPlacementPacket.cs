
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
    public class PlayerBlockPlacementPacket : ProtobufPacket
    {
		public Position Location;
		public SByte Face;
		public ItemStack HeldItem;
		public SByte CursorPositionX;
		public SByte CursorPositionY;
		public SByte CursorPositionZ;

        public override VarInt ID { get { return 8; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Location = reader.Read(Location);
			Face = reader.Read(Face);
			HeldItem = reader.Read(HeldItem);
			CursorPositionX = reader.Read(CursorPositionX);
			CursorPositionY = reader.Read(CursorPositionY);
			CursorPositionZ = reader.Read(CursorPositionZ);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Location);
			stream.Write(Face);
			stream.Write(HeldItem);
			stream.Write(CursorPositionX);
			stream.Write(CursorPositionY);
			stream.Write(CursorPositionZ);
          
            return this;
        }

    }
}