using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets
{
    public class HandshakePacket : IPacket
    {
        public VarInt ProtocolVersion;
        public string ServerAddress;
        public ushort ServerPort;
        public NextState NextState;

        public byte ID { get { return 0x00; } }

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
            stream.WriteVarInt(ProtocolVersion);
            stream.WriteString(ServerAddress);
            stream.WriteUShort(ServerPort);
            stream.WriteVarInt((int) NextState);
            
            return this;
        }
    }
}