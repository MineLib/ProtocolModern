using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;

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
            stream.WriteVarInt(ID);
            stream.WriteByte(WindowID);
            ItemStackList.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}