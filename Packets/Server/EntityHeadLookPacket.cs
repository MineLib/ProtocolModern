using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct EntityHeadLookPacket : IPacket
    {
        public int EntityID { get; set; }
        public sbyte HeadYaw { get; set; }

        public byte ID { get { return 0x19; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            HeadYaw = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(EntityID);
            stream.WriteSByte(HeadYaw);
            
            return this;
        }
    }
}