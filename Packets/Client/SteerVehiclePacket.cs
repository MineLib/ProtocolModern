using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct SteerVehiclePacket : IPacket
    {
        public float Sideways { get; set; }
        public float Forward { get; set; }
        public SteerVehicle Flags { get; set; }

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
            stream.WriteFloat(Sideways);
            stream.WriteFloat(Forward);
            stream.WriteByte((byte) Flags);
            
            return this;
        }
    }
}