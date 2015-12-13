
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

namespace MineLib.PacketBuilder.Client.Play
{
    public class PluginMessagePacket : ProtobufPacket
    {
		public String Channel;
		public Byte[] Data;

        public override VarInt ID { get { return 63; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			Channel = reader.Read(Channel);
			var DataLength = reader.Read<VarInt>();
			Data = reader.Read(Data, DataLength);

            return this;
        }

        public override ProtobufPacket WritePacket(PacketStream stream)
        {
			stream.Write(Channel);
			stream.Write(Data.Length);
			stream.Write(Data);
          
            return this;
        }

    }
}