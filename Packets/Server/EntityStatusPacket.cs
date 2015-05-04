using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct EntityStatusPacket : IPacket
    {
        public int EntityID;
        public EntityStatus Status;

        public byte ID { get { return 0x1A; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadInt();
            Status = (EntityStatus) reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteInt(EntityID);
            stream.WriteSByte((sbyte) Status);
            stream.Purge();

            return this;
        }
    }
}