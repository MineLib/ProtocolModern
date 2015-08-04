using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct PlayerBlockPlacementPacket : IPacket
    {
        public Position Location { get; set; }
        public Direction Direction { get; set; }
        public ItemStack Slot { get; set; }
        public Position CursorVector3 { get; set; }

        public byte ID { get { return 0x08; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Direction = (Direction) reader.ReadSByte();
            Slot = ItemStack.FromReader(reader);
            CursorVector3 = Vector3.FromReaderSByte(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            Location.ToStreamLong(stream);
            stream.WriteSByte((sbyte) Direction);
            Slot.ToStream(stream);
            CursorVector3.ToStreamSByte(stream);
            
            return this;
        }
    }
}