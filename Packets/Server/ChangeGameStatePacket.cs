using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct ChangeGameStatePacket : IPacket
    {
        public GameStateReason Reason { get; set; }
        public float Value { get; set; }

        public byte ID { get { return 0x2B; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Reason = (GameStateReason) reader.ReadByte();
            Value = reader.ReadFloat();

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte((byte) Reason);
            stream.WriteFloat(Value);
            
            return this;
        }
    }
}