
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
    public class SoundEffectPacket : ProtobufPacket
    {
		public String Soundname;
		public Int32 EffectpositionX;
		public Int32 EffectpositionY;
		public Int32 EffectpositionZ;
		public Single Volume;
		public Byte Pitch;

        public override VarInt ID { get { return 41; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Soundname = reader.Read(Soundname);
			EffectpositionX = reader.Read(EffectpositionX);
			EffectpositionY = reader.Read(EffectpositionY);
			EffectpositionZ = reader.Read(EffectpositionZ);
			Volume = reader.Read(Volume);
			Pitch = reader.Read(Pitch);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Soundname);
			stream.Write(EffectpositionX);
			stream.Write(EffectpositionY);
			stream.Write(EffectpositionZ);
			stream.Write(Volume);
			stream.Write(Pitch);
          
            return this;
        }

    }
}