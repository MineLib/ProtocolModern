using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct EntityPacket : IPacket
    {
        public int EntityID;

        public byte ID { get { return 0x14; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(EntityID);
            
            return this;
        }
    }
}