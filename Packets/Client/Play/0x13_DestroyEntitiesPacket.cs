
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
    public class DestroyEntitiesPacket : ProtobufPacket
    {
		public VarInt Count;
		public VarInt[] EntityIDs;

        public override VarInt ID { get { return 19; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			var EntityIDsLength = reader.Read<VarInt>();
			EntityIDs = reader.Read(EntityIDs, EntityIDsLength);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityIDs.Length);
			stream.Write(EntityIDs);
          
            return this;
        }

    }
}