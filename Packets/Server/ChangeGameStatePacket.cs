using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct ChangeGameStatePacket : IPacket
    {
        public GameStateReason Reason;
        public float Value;

        public byte ID { get { return 0x2B; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Reason = (GameStateReason) reader.ReadByte();
            Value = reader.ReadFloat();

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte((byte) Reason);
            stream.WriteFloat(Value);
            stream.Purge();

            return this;
        }
    }
}