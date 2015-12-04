
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Server.Login
{
    public class EncryptionResponsePacket : ProtobufPacket
    {
		public VarInt SharedSecretLength;
		public Byte[] SharedSecret;
		public VarInt VerifyTokenLength;
		public Byte[] VerifyToken;

        public override VarInt ID { get { return 1; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			var SharedSecretLength = reader.Read<VarInt>();
			SharedSecret = reader.Read(SharedSecret, SharedSecretLength);
			var VerifyTokenLength = reader.Read<VarInt>();
			VerifyToken = reader.Read(VerifyToken, VerifyTokenLength);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(SharedSecret.Length);
			stream.Write(SharedSecret);
			stream.Write(VerifyToken.Length);
			stream.Write(VerifyToken);
          
            return this;
        }

    }
}