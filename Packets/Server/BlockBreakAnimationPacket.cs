using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct BlockBreakAnimationPacket : IPacket
    {
        public int EntityID { get; set; }
        public Position Location { get; set; }
        public sbyte DestroyStage { get; set; }

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
            stream.WriteVarInt(EntityID);
            Location.ToStreamLong(stream);
            stream.WriteSByte(DestroyStage);
            
            return this;
        }
    }
}