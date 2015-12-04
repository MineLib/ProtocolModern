
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Server.Play
{
    public class EnchantItemPacket : ProtobufPacket
    {
		public SByte WindowID;
		public SByte Enchantment;

        public override VarInt ID { get { return 17; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			WindowID = reader.Read(WindowID);
			Enchantment = reader.Read(Enchantment);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(WindowID);
			stream.Write(Enchantment);
          
            return this;
        }

    }
}