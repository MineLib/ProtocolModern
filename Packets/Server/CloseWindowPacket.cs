using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct CloseWindowPacket : IPacket
    {
        public byte WindowID;

        public byte ID { get { return 0x2E; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowID = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            stream.Purge();

            return this;
        }
    }
}