
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
    public class PluginMessage2Packet : ProtobufPacket
    {
		public String Channel;
		public Byte[] Data;

        public override VarInt ID { get { return 23; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Channel = reader.Read(Channel);
			var DataLength = reader.Read<VarInt>();
			Data = reader.Read(Data, DataLength);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(Channel);
			stream.Write(Data.Length);
			stream.Write(Data);
          
            return this;
        }

    }
}