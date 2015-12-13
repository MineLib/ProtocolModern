
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
using ProtocolModern.Extensions;

namespace MineLib.PacketBuilder.Client.Play
{
    public class CombatEventPacket : ProtobufPacket
    {
		public VarInt Event;
		public NotSupportedType Duration;
		public NotSupportedType PlayerID;
		public NotSupportedType EntityID;
		public String Message;

        public override VarInt ID { get { return 66; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Event = reader.Read(Event);
			Duration = reader.Read(Duration);
			PlayerID = reader.Read(PlayerID);
			EntityID = reader.Read(EntityID);
			Message = reader.Read(Message);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Event);
			stream.Write(Duration);
			stream.Write(PlayerID);
			stream.Write(EntityID);
			stream.Write(Message);
          
            return this;
        }

    }
}