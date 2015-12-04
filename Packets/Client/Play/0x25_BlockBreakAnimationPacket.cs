
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
    public class BlockBreakAnimationPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Position Location;
		public SByte DestroyStage;

        public override VarInt ID { get { return 37; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Location = reader.Read(Location);
			DestroyStage = reader.Read(DestroyStage);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Location);
			stream.Write(DestroyStage);
          
            return this;
        }

    }
}