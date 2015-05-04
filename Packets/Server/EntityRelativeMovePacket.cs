using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct EntityRelativeMovePacket : IPacket
    {
        public int EntityID;
        public Vector3 DeltaVector3;
        public bool OnGround;

        public byte ID { get { return 0x15; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            DeltaVector3 = Vector3.FromReaderSByteFixedPoint(reader);
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            DeltaVector3.ToStreamSByteFixedPoint(stream);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}