using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct EntityActionPacket : IPacket
    {
        public VarInt EntityID { get; set; }
        public EntityAction Action { get; set; }
        public VarInt JumpBoost { get; set; }

        public byte ID { get { return 0x0B; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Action = (EntityAction) (int) reader.ReadVarInt();
            JumpBoost = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(EntityID);
            stream.WriteVarInt((int) Action);
            stream.WriteVarInt(JumpBoost);
            
            return this;
        }
    }
}