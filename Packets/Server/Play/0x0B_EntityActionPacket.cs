
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
    public class EntityActionPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public VarInt ActionID;
		public VarInt JumpBoost;

        public override VarInt ID { get { return 11; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			ActionID = reader.Read(ActionID);
			JumpBoost = reader.Read(JumpBoost);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(ActionID);
			stream.Write(JumpBoost);
          
            return this;
        }

    }
}