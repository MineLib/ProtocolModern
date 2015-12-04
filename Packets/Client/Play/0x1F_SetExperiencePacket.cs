
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
    public class SetExperiencePacket : ProtobufPacket
    {
		public Single Experiencebar;
		public VarInt Level;
		public VarInt TotalExperience;

        public override VarInt ID { get { return 31; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Experiencebar = reader.Read(Experiencebar);
			Level = reader.Read(Level);
			TotalExperience = reader.Read(TotalExperience);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Experiencebar);
			stream.Write(Level);
			stream.Write(TotalExperience);
          
            return this;
        }

    }
}