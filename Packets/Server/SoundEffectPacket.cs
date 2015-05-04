using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct SoundEffectPacket : IPacket
    {
        public string SoundName;
        public Position Coordinates;
        public float Volume;
        public byte Pitch;

        public byte ID { get { return 0x29; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            SoundName = reader.ReadString();
            Coordinates = Position.FromReaderInt(reader);
            Volume = reader.ReadFloat();
            Pitch = reader.ReadByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(SoundName);
            Coordinates.ToStreamInt(stream);
            stream.WriteFloat(Volume);
            stream.WriteByte(Pitch);
            stream.Purge();

            return this;
        }
    }
}