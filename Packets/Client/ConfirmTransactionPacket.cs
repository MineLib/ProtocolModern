using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public byte WindowID;
        public short Slot;
        public bool Accepted;

        public byte ID { get { return 0x0F; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowID = reader.ReadByte();
            Slot = reader.ReadShort();
            Accepted = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(WindowID);
            stream.WriteShort(Slot);
            stream.WriteBoolean(Accepted);
            
            return this;
        }
    }
}