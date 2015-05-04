using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct EntityHeadLookPacket : IPacket
    {
        public int EntityID;
        public sbyte HeadYaw;

        public byte ID { get { return 0x19; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            HeadYaw = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte(HeadYaw);
            stream.Purge();

            return this;
        }
    }
}