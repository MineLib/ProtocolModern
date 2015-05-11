using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct ScoreboardObjectivePacket : IPacket
    {
        public string ObjectiveName;
        public sbyte Mode;
        public string ObjectiveValue;
        public string Type;

        public byte ID { get { return 0x3B; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            ObjectiveName = reader.ReadString();
            Mode = reader.ReadSByte();
            ObjectiveValue = reader.ReadString();
            Type = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteString(ObjectiveName);
            stream.WriteSByte(Mode);
            stream.WriteString(ObjectiveValue);
            stream.WriteString(Type);
            
            return this;
        }
    }
}