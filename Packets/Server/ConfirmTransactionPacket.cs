using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public byte WindowId { get; set; }
        public short ActionNumber { get; set; }
        public bool Accepted { get; set; }

        public byte ID { get { return 0x32; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowId = reader.ReadByte();
            ActionNumber = reader.ReadShort();
            Accepted = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(WindowId);
            stream.WriteShort(ActionNumber);
            stream.WriteBoolean(Accepted);
            
            return this;
        }
    }
}