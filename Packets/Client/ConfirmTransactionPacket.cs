using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public sbyte WindowID { get; set; }
        public short Slot { get; set; }
        public bool Accepted { get; set; }

        public byte ID { get { return 0x0F; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowID = reader.ReadSByte();
            Slot = reader.ReadShort();
            Accepted = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteSByte(WindowID);
            stream.WriteShort(Slot);
            stream.WriteBoolean(Accepted);
            
            return this;
        }
    }
}