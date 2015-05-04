using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct StatisticsPacket : IPacket
    {
        public StatisticsEntryList StatisticsEntryList;

        public byte ID { get { return 0x37; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            StatisticsEntryList = StatisticsEntryList.FromReader(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            StatisticsEntryList.ToStream(stream);
            stream.Purge();

            return this;
        }
    }
}