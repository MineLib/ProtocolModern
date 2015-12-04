
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
    public class AttachEntityPacket : ProtobufPacket
    {
		public Int32 EntityID;
		public Int32 VehicleID;
		public Boolean Leash;

        public override VarInt ID { get { return 27; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			EntityID = reader.Read(EntityID);
			VehicleID = reader.Read(VehicleID);
			Leash = reader.Read(Leash);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(EntityID);
			stream.Write(VehicleID);
			stream.Write(Leash);
          
            return this;
        }

    }
}