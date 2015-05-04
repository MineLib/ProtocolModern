using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct PlayerLookPacket : IPacket
    {
        public float Yaw;
        public float Pitch;
        public bool OnGround;

        public byte ID { get { return 0x05; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Yaw = reader.ReadFloat();
            Pitch = reader.ReadFloat();
            OnGround = reader.ReadBoolean();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(Yaw);
            stream.WriteFloat(Pitch);
            stream.WriteBoolean(OnGround);
            stream.Purge();

            return this;
        }
    }
}