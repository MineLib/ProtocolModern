using MineLib.Core;
using MineLib.Core.Data.Structs;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct WindowItemsPacket : IPacket
    {
        public byte WindowID;
        public ItemStackList ItemStackList;

        public byte ID { get { return 0x30; } }
    
        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            WindowID = reader.ReadByte();
            ItemStackList = ItemStackList.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteByte(WindowID);
            ItemStackList.ToStream(stream);
            
            return this;
        }
    }
}