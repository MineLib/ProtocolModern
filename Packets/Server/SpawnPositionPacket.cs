using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct SpawnPositionPacket : IPacket
    {
        public Position Location;

        public byte ID { get { return 0x05; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromReaderLong(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            stream.Purge();

            return this;
        }
    }
}