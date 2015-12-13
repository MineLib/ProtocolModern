
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
    public class UpdateBlockEntityPacket : ProtobufPacket
    {
		public Position Location;
		public Byte Action;
		public NotSupportedType NBTData;

        public override VarInt ID { get { return 53; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Location = reader.Read(Location);
			Action = reader.Read(Action);
			NBTData = reader.Read(NBTData);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Location);
			stream.Write(Action);
			stream.Write(NBTData);
          
            return this;
        }

    }
}