using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct UpdateEntityNBTPacket : IPacket
    {
        public int EntityID;
        public byte[] NBTTag;

        public byte ID { get { return 0x49; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            var length = reader.ReadVarInt(); // TODO: Check that
            NBTTag = reader.ReadByteArray(length);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            stream.WriteVarInt(NBTTag.Length);
            stream.WriteByteArray(NBTTag);
            stream.Purge();

            return this;
        }
    }
}
