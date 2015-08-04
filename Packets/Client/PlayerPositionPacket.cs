using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct PlayerPositionPacket : IPacket
    {
        public double X { get; set; }
        public double FeetY { get; set; }
        public double Z { get; set; }
        public bool OnGround { get; set; }

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
            stream.WriteDouble(X);
            stream.WriteDouble(FeetY);
            stream.WriteDouble(Z);
            stream.WriteBoolean(OnGround);
            
            return this;
        }
    }
}