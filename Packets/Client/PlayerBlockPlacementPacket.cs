using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.Data.Structs;
using MineLib.Network.IO;
using ProtocolModern.Enum;

namespace ProtocolModern.Packets.Client
{
    public struct PlayerBlockPlacementPacket : IPacket
    {
        public Position Location;
        public Direction Direction;
        public ItemStack Slot;
        public Position CursorVector3;

        public byte ID { get { return 0x08; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Direction = (Direction) reader.ReadByte();
            Slot = ItemStack.FromReader(reader);
            CursorVector3 = Vector3.FromReaderByte(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            stream.WriteByte((byte) Direction);
            Slot.ToStream(stream);
            CursorVector3.ToStreamByte(stream);
            stream.Purge();

            return this;
        }
    }
}