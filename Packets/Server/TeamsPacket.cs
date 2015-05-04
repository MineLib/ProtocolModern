using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public interface ITeam
    {
        ITeam FromReader(IProtocolDataReader reader);
        void ToStream(IProtocolStream stream);
    }

    public struct TeamsCreateTeam : ITeam
    {
        public string TeamDisplayName;
        public string TeamPrefix;
        public string TeamSuffix;
        public byte FriendlyFire;
        public string NameTagVisibility;
        public byte Color;
        public string[] Players;

        public ITeam FromReader(IProtocolDataReader reader)
        {
            TeamDisplayName = reader.ReadString();
            TeamPrefix = reader.ReadString();
            TeamSuffix = reader.ReadString();
            FriendlyFire = reader.ReadByte();
            NameTagVisibility = reader.ReadString();
            Color = reader.ReadByte();

            var count = reader.ReadVarInt();
            Players = new string[count];
            for (var i = 0; i < count; i++)
                Players[i] = reader.ReadString();
            

            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteString(TeamDisplayName);
            stream.WriteString(TeamPrefix);
            stream.WriteString(TeamSuffix);
            stream.WriteByte(FriendlyFire);
            stream.WriteString(NameTagVisibility);
            stream.WriteByte(Color);
            stream.WriteVarInt(Players.Length);
            stream.WriteStringArray(Players);
        }
    }

    public struct TeamsRemoveTeam : ITeam
    {
        public ITeam FromReader(IProtocolDataReader reader)
        {
            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
        }
    }

    public struct TeamsUpdateTeam : ITeam
    {
        public string TeamDisplayName;
        public string TeamPrefix;
        public string TeamSuffix;
        public byte FriendlyFire;
        public string NameTagVisibility;
        public byte Color;

        public ITeam FromReader(IProtocolDataReader reader)
        {
            TeamDisplayName = reader.ReadString();
            TeamPrefix = reader.ReadString();
            TeamSuffix = reader.ReadString();
            FriendlyFire = reader.ReadByte();
            NameTagVisibility = reader.ReadString();
            Color = reader.ReadByte();

            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteString(TeamDisplayName);
            stream.WriteString(TeamPrefix);
            stream.WriteString(TeamSuffix);
            stream.WriteByte(FriendlyFire);
            stream.WriteString(NameTagVisibility);
            stream.WriteByte(Color);
        }
    }

    public struct TeamsAddPlayers : ITeam
    {
        public string[] Players;

        public ITeam FromReader(IProtocolDataReader reader)
        {
            var count = reader.ReadVarInt();
            Players = new string[count];
            for (var i = 0; i < count; i++)
                Players[i] = reader.ReadString();
            

            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteVarInt(Players.Length);
            stream.WriteStringArray(Players);
        }
    }

    public struct TeamsRemovePlayers : ITeam
    {
        public string[] Players;

        public ITeam FromReader(IProtocolDataReader reader)
        {
            var count = reader.ReadVarInt();
            Players = new string[count];
            for (var i = 0; i < count; i++)
                Players[i] = reader.ReadString();
            

            return this;
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteVarInt(Players.Length);
            stream.WriteStringArray(Players);
        }
    }

    public struct TeamsPacket : IPacket
    {
        public string TeamName;
        public TeamAction Action;
        public ITeam Team;

        public byte ID { get { return 0x3E; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            TeamName = reader.ReadString();
            Action = (TeamAction) reader.ReadByte();

            switch (Action)
            {
                case TeamAction.CreateTeam:
                    Team = new TeamsCreateTeam().FromReader(reader);
                    break;
                case TeamAction.RemoveTeam:
                    Team = new TeamsRemoveTeam().FromReader(reader);
                    break;
                case TeamAction.UpdateTeam:
                    Team = new TeamsUpdateTeam().FromReader(reader);
                    break;
                case TeamAction.AddPlayers:
                    Team = new TeamsAddPlayers().FromReader(reader);
                    break;
                case TeamAction.RemovePlayers:
                    Team = new TeamsRemovePlayers().FromReader(reader);
                    break;
            }

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(TeamName);
            stream.WriteByte((byte) Action);
            Team.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}
