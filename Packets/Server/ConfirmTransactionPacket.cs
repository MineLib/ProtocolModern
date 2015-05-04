using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct ConfirmTransactionPacket : IPacket
    {
        public byte WindowId;
        public short ActionNumber;
        public bool Accepted;

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
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowId);
            stream.WriteShort(ActionNumber);
            stream.WriteBoolean(Accepted);
            stream.Purge();

            return this;
        }
    }
}