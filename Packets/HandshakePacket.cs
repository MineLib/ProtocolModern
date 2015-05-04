using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets
{
    public struct HandshakePacket : IPacket
    {
        public int ProtocolVersion;
        public string ServerAddress;
        public ushort ServerPort;
        public NextState NextState;

        public const byte PacketID = 0x00;
        public byte ID { get { return PacketID; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            ProtocolVersion = reader.ReadVarInt();
            ServerAddress = reader.ReadString();
            ServerPort = reader.ReadUShort();
            NextState = (NextState) (int) reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerAddress);
            stream.WriteUShort(ServerPort);
            stream.WriteVarInt((byte) NextState);
            stream.Purge();

            return this;
        }
    }
}