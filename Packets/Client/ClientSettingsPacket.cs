using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct ClientSettingsPacket : IPacket
    {
        public string Locale;
        public byte ViewDistance;
        public ChatFlags ChatFlags;
        public bool ChatColours;
        public DisplayedSkinParts DisplayedSkinParts;

        public byte ID { get { return 0x15; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Locale = reader.ReadString();
            ViewDistance = reader.ReadByte();
            ChatFlags = (ChatFlags) reader.ReadByte();
            ChatColours = reader.ReadBoolean();
            DisplayedSkinParts = DisplayedSkinParts.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Locale);
            stream.WriteByte(ViewDistance);
            stream.WriteByte((byte) ChatFlags);
            stream.WriteBoolean(ChatColours);
            DisplayedSkinParts.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}