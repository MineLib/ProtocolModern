
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
    public class ChangeGameStatePacket : ProtobufPacket
    {
		public Byte Reason;
		public Single Value;

        public override VarInt ID { get { return 43; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Reason = reader.Read(Reason);
			Value = reader.Read(Value);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Reason);
			stream.Write(Value);
          
            return this;
        }

    }
}