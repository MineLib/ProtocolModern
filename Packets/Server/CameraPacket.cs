using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct CameraPacket : IPacket
    {
        public int CameraID { get; set; }

        public byte ID { get { return 0x43; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            CameraID = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(CameraID);
            
            return this;
        }
    }
}
