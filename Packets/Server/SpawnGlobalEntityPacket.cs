using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct SpawnGlobalEntityPacket : IPacket
    {
        public int EntityID;
        public sbyte Type;
        public Vector3 Vector3;

        public byte ID { get { return 0x2C; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Type = reader.ReadSByte();
            Vector3 = Vector3.FromReaderIntFixedPoint(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte(Type);
            Vector3.ToStreamIntFixedPoint(stream);
            stream.Purge();

            return this;
        }
    }
}