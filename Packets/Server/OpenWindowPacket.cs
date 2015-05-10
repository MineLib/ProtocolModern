using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct OpenWindowPacket : IPacket
    {
        public byte WindowID;
        public string InventoryType;
        public string WindowTitle;
        public byte NumberOfSlots;
        public int? EntityID;

        public byte ID { get { return 0x2D; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowID = reader.ReadByte();
            InventoryType = reader.ReadString();
            WindowTitle = reader.ReadString();
            NumberOfSlots = reader.ReadByte();
            if (InventoryType == "EntityHorse")
                EntityID = reader.ReadInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            stream.WriteString(InventoryType);
            stream.WriteString(WindowTitle);
            stream.WriteByte(NumberOfSlots);
            if (InventoryType == "EntityHorse")
                stream.WriteInt(EntityID.GetValueOrDefault());
            
            return this;
        }
    }
}