using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct DisplayScoreboardPacket : IPacket
    {
        public ScoreboardPosition Position { get; set; }
        public string ScoreName { get; set; }

        public byte ID { get { return 0x3D; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Position = (ScoreboardPosition) reader.ReadSByte();
            ScoreName = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteSByte((sbyte) Position);
            stream.WriteString(ScoreName);
            
            return this;
        }
    }
}