using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct CloseWindowPacket : IPacket
    {
        public sbyte WindowID { get; set; }

        public byte ID { get { return 0x0D; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowID = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteSByte(WindowID);
            
            return this;
        }
    }
}