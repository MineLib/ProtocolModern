using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct PlayerAbilitiesPacket : IPacket
    {
        public sbyte Flags;
        public float FlyingSpeed;
        public float WalkingSpeed;

        public byte ID { get { return 0x39; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Flags = reader.ReadSByte();
            FlyingSpeed = reader.ReadFloat();
            WalkingSpeed = reader.ReadFloat();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte(Flags);
            stream.WriteFloat(FlyingSpeed);
            stream.WriteFloat(WalkingSpeed);
            stream.Purge();

            return this;
        }
    }
}