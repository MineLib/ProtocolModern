using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct CloseWindowPacket : IPacket
    {
        public byte WindowID { get; set; }

        public byte ID { get { return 0x2E; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowID = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(WindowID);
            
            return this;
        }
    }
}