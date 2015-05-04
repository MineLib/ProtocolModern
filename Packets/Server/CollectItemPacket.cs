using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct CollectItemPacket : IPacket
    {
        public int CollectedEntityID;
        public int CollectorEntityID;

        public byte ID { get { return 0x0D; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            CollectedEntityID = reader.ReadVarInt();
            CollectorEntityID = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(CollectedEntityID);
            stream.WriteVarInt(CollectorEntityID);
            stream.Purge();

            return this;
        }
    }
}