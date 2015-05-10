using MineLib.Core;
using MineLib.Core.Data.Structs;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    //TODO: Broken
    public struct EntityMetadataPacket : IPacket
    {
        public int EntityID;
        public EntityMetadataList Metadata;

        public byte ID { get { return 0x1C; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            //Metadata = EntityMetadataList.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Metadata.ToStream(stream);
            
            return this;
        }
    }
}