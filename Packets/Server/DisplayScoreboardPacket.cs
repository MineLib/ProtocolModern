using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct DisplayScoreboardPacket : IPacket
    {
        public ScoreboardPosition Position;
        public string ScoreName;

        public byte ID { get { return 0x3D; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Position = (ScoreboardPosition) reader.ReadSByte();
            ScoreName = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteSByte((sbyte) Position);
            stream.WriteString(ScoreName);
            stream.Purge();

            return this;
        }
    }
}