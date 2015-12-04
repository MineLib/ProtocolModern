
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;
using ProtocolModern.Extensions;

namespace MineLib.PacketBuilder.Server.Play
{
    public class SpectatePacket : ProtobufPacket
    {
		public NotSupportedType TargetPlayer;

        public override VarInt ID { get { return 24; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			TargetPlayer = reader.Read(TargetPlayer);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(TargetPlayer);
          
            return this;
        }

    }
}