
using Aragas.Core.Data;
using Aragas.Core.Extensions;
using Aragas.Core.Interfaces;
using Aragas.Core.Packets;

using MineLib.Core.Data;
using MineLib.Core.Data.Anvil;
using MineLib.Core.Data.Structs;
using MineLib.Core.Extensions;

using System;

namespace MineLib.PacketBuilder.Client.Login
{
    public class EncryptionRequestPacket : ProtobufPacket
    {
		public String ServerID;
		public VarInt PublicKeyLength;
		public Byte[] PublicKey;
		public VarInt VerifyTokenLength;
		public Byte[] VerifyToken;

        public override VarInt ID { get { return 1; } }

        public override ProtobufPacket ReadPacket(PacketDataReader reader)
        {
			ServerID = reader.Read(ServerID);
			var PublicKeyLength = reader.Read<VarInt>();
			PublicKey = reader.Read(PublicKey, PublicKeyLength);
			var VerifyTokenLength = reader.Read<VarInt>();
			VerifyToken = reader.Read(VerifyToken, VerifyTokenLength);

            return this;
        }

        public override ProtobufPacket WritePacket(IPacketStream stream)
        {
			stream.Write(ServerID);
			stream.Write(PublicKey.Length);
			stream.Write(PublicKey);
			stream.Write(VerifyToken.Length);
			stream.Write(VerifyToken);
          
            return this;
        }

    }
}