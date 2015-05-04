using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct BlockBreakAnimationPacket : IPacket
    {
        public int EntityID;
        public Position Location;
        public sbyte DestroyStage;

        public byte ID { get { return 0x25; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            EntityID = reader.ReadVarInt();
            Location = Position.FromReaderLong(reader);
            DestroyStage = reader.ReadSByte();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(EntityID);
            Location.ToStreamLong(stream);
            stream.WriteSByte(DestroyStage);
            stream.Purge();

            return this;
        }
    }
}