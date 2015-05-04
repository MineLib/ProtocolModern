using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct SteerVehiclePacket : IPacket
    {
        public float Sideways;
        public float Forward;
        public SteerVehicle Flags;

        public byte ID { get { return 0x0C; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Sideways = reader.ReadFloat();
            Forward = reader.ReadFloat();
            Flags = (SteerVehicle) reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteFloat(Sideways);
            stream.WriteFloat(Forward);
            stream.WriteByte((byte) Flags);
            stream.Purge();

            return this;
        }
    }
}