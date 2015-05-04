using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public interface ICombatEvent
    {
        ICombatEvent FromReader(IProtocolDataReader reader);
        void ToStream(IProtocolStream stream);
    }

    public struct CombatEventEnterCombat : ICombatEvent
    {
        public ICombatEvent FromReader(IProtocolDataReader reader)
        {
            return this; // Hope works TODO: Check this
        }

        public void ToStream(IProtocolStream stream)
        {
        }
    }

    public struct CombatEventEndCombat : ICombatEvent
    {
        public int Duration;
        public int EntityID;

        public ICombatEvent FromReader(IProtocolDataReader reader)
        {
            Duration = reader.ReadVarInt();
            EntityID = reader.ReadInt();

            return this; // Hope works
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteVarInt(Duration);
            stream.WriteInt(EntityID);
        }
    }

    public struct CombatEventEntityDead : ICombatEvent
    {
        public int PlayerID;
        public int EntityID;
        public string Message;

        public ICombatEvent FromReader(IProtocolDataReader reader)
        {
            PlayerID = reader.ReadVarInt();
            EntityID = reader.ReadInt();
            Message = reader.ReadString();

            return this; // Hope works
        }

        public void ToStream(IProtocolStream stream)
        {
            stream.WriteVarInt(PlayerID);
            stream.WriteInt(EntityID);
            stream.WriteString(Message);
        }
    }

    public struct CombatEventPacket : IPacket
    {
        public int Event;

        public ICombatEvent CombatEvent;

        public byte ID { get { return 0x42; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Event = reader.ReadVarInt();

            switch (Event)
            {
                case 0:
                    CombatEvent = new CombatEventEnterCombat().FromReader(reader);
                    break;

                case 1:
                    CombatEvent = new CombatEventEndCombat().FromReader(reader);
                    break;

                case 2:
                    CombatEvent = new CombatEventEntityDead().FromReader(reader);
                    break;
            }


            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Event);
            CombatEvent.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}
