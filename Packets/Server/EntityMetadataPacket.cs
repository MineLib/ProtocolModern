using System;
using MineLib.Core.Data.Structs;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    //TODO: Broken ?
    public struct EntityMetadataPacket : IPacket
    {
        public int EntityID { get; set; }
        public EntityMetadataList Metadata { get; set; }

        public byte ID { get { return 0x1C; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Metadata = EntityMetadataList.FromReader(reader);

            return this;
        }
    
        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(EntityID);
            Metadata.ToStream(stream);
            
            return this;
        }
    }
}