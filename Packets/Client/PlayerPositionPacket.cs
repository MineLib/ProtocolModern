using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct PlayerPositionPacket : IPacket
    {
        public double X, FeetY, Z;
        public bool OnGround;

        public byte ID { get { return 0x04; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            X = reader.ReadDouble();
            FeetY = reader.ReadDouble();
            Z = reader.ReadDouble();
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(Z);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}