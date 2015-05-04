using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct SpawnExperienceOrbPacket : IPacket
    {
        public int EntityID;
        public Vector3 Vector3;
        public short Count;

        public byte ID { get { return 0x11; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);
            Count = reader.ReadShort();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Vector3.ToStreamIntFixedPoint(stream);
            stream.WriteShort(Count);
            stream.Purge();

            return this;
        }
    }
}