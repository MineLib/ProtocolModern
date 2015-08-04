using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct PlayerDiggingPacket : IPacket
    {
        public BlockStatus Status { get; set; }
        public Position Location { get; set; }
        public sbyte Face { get; set; }

        public byte ID { get { return 0x07; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Status = (BlockStatus) reader.ReadSByte();
            Location = Position.FromReaderLong(reader);
            Face = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteSByte((sbyte) Status);
            Location.ToStreamLong(stream);
            stream.WriteSByte(Face);
            
            return this;
        }
    }
}