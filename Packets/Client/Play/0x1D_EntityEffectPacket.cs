
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
    public class EntityEffectPacket : ProtobufPacket
    {
		public VarInt EntityID;
		public SByte EffectID;
		public SByte Amplifier;
		public VarInt Duration;
		public Boolean HideParticles;

        public override VarInt ID { get { return 29; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			EffectID = reader.Read(EffectID);
			Amplifier = reader.Read(Amplifier);
			Duration = reader.Read(Duration);
			HideParticles = reader.Read(HideParticles);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(EffectID);
			stream.Write(Amplifier);
			stream.Write(Duration);
			stream.Write(HideParticles);
          
            return this;
        }

    }
}