
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
    public class CollectItemPacket : ProtobufPacket
    {
		public VarInt CollectedEntityID;
		public VarInt CollectorEntityID;

        public override VarInt ID { get { return 13; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			CollectedEntityID = reader.Read(CollectedEntityID);
			CollectorEntityID = reader.Read(CollectorEntityID);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(CollectedEntityID);
			stream.Write(CollectorEntityID);
          
            return this;
        }

    }
}