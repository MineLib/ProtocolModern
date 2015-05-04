using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct EntityLookAndRelativeMovePacket : IPacket
    {
        public int EntityID;
        public Vector3 DeltaVector3;
        public sbyte Yaw;
        public sbyte Pitch;
        public bool OnGround;

        public byte ID { get { return 0x17; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            DeltaVector3 = Vector3.FromReaderSByteFixedPoint(reader);
            Yaw = reader.ReadSByte();
            Pitch = reader.ReadSByte();
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            DeltaVector3.ToStreamSByteFixedPoint(stream);
            stream.WriteSByte(Yaw);
            stream.WriteSByte(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}