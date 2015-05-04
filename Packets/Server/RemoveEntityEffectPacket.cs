using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Server
{
    public struct RemoveEntityEffectPacket : IPacket
    {
        public int EntityID;
        public EffectID EffectID;

        public byte ID { get { return 0x1E; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            EffectID = (EffectID) reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteSByte((sbyte) EffectID);
            stream.Purge();

            return this;
        }
    }
}