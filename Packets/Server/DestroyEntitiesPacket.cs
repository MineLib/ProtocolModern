using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct DestroyEntitiesPacket : IPacket
    {
        public int[] EntityIDs;

        public byte ID { get { return 0x13; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            var count = reader.ReadVarInt();
            EntityIDs = reader.ReadVarIntArray(count);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityIDs.Length);
            stream.WriteVarIntArray(EntityIDs);
            stream.Purge();

            return this;
        }
    }
}