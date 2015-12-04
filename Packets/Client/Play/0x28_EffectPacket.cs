
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
    public class EffectPacket : ProtobufPacket
    {
		public Int32 EffectID;
		public Position Location;
		public Int32 Data;
		public Boolean DisableRelativeVolume;

        public override VarInt ID { get { return 40; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EffectID = reader.Read(EffectID);
			Location = reader.Read(Location);
			Data = reader.Read(Data);
			DisableRelativeVolume = reader.Read(DisableRelativeVolume);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EffectID);
			stream.Write(Location);
			stream.Write(Data);
			stream.Write(DisableRelativeVolume);
          
            return this;
        }

    }
}