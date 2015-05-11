using MineLib.Core;
using MineLib.Core.Data;
using MineLib.Core.Data.Structs;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct MultiBlockChangePacket : IPacket
    {
        public Coordinates2D Coordinates; // TODO: Add FromReader() ?
        public RecordList RecordList;

        public byte ID { get { return 0x22; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Coordinates = Coordinates2D.FromReaderInt(reader);
            RecordList = RecordList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            Coordinates.ToStreamInt(stream);
            RecordList.ToStream(stream);
            
            return this;
        }
    }
}