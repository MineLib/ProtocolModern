using MineLib.Core;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;
using MineLib.Core.Wrappers;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct JoinGamePacket : IPacket
    {
        public int EntityID;
        public GameMode GameMode;
        public Dimension Dimension;
        public Difficulty Difficulty;
        public byte MaxPlayers;
        public string LevelType;
        public bool ReducedDebugInfo;

        public byte ID { get { return 0x01; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadInt();
            GameMode = (GameMode) reader.ReadByte();
            Dimension = (Dimension) reader.ReadSByte();
            Difficulty = (Difficulty) reader.ReadByte();
            MaxPlayers = reader.ReadByte();
            LevelType = reader.ReadString();
            ReducedDebugInfo = reader.ReadBoolean();

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteInt(EntityID);
            stream.WriteVarInt((byte) GameMode);
            stream.WriteSByte((sbyte) Dimension);
            stream.WriteVarInt((byte) Difficulty);
            stream.WriteByte(MaxPlayers);
            stream.WriteString(LevelType);
            stream.WriteBoolean(ReducedDebugInfo);
            
            return this;
        }
    }
}