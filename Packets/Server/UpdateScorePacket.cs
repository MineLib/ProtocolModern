using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct UpdateScorePacket : IPacket
    {
        public string ScoreName;
        public bool RemoveItem; // Will be converted to byte 0-1
        public string ObjectiveName;
        public int? Value;

        public byte ID { get { return 0x3C; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            ScoreName = reader.ReadString();
            RemoveItem = reader.ReadBoolean();
            if (RemoveItem)
            {
                ObjectiveName = reader.ReadString();
                Value = reader.ReadInt();
            }

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(ScoreName);
            stream.WriteBoolean(RemoveItem);
            if (!RemoveItem)
            {
                stream.WriteString(ObjectiveName);
                stream.WriteInt(Value.Value);
            }
            stream.Purge();

            return this;
        }
    }
}