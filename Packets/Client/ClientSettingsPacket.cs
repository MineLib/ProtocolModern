using MineLib.Core.Data.Structs;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct ClientSettingsPacket : IPacket
    {
        public string Locale { get; set; }
        public sbyte ViewDistance { get; set; }
        public ChatFlags ChatFlags { get; set; }
        public bool ChatColours { get; set; }
        public DisplayedSkinParts DisplayedSkinParts { get; set; }

        public byte ID { get { return 0x15; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Locale = reader.ReadString();
            ViewDistance = reader.ReadSByte();
            ChatFlags = (ChatFlags) reader.ReadSByte();
            ChatColours = reader.ReadBoolean();
            DisplayedSkinParts = DisplayedSkinParts.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteString(Locale);
            stream.WriteSByte(ViewDistance);
            stream.WriteSByte((sbyte) ChatFlags);
            stream.WriteBoolean(ChatColours);
            DisplayedSkinParts.ToStream(stream);
            
            return this;
        }
    }
}