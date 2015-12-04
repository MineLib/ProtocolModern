
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
    public class AnimationPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public Byte Animation;

        public override VarInt ID { get { return 11; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			Animation = reader.Read(Animation);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(Animation);
          
            return this;
        }

    }
}