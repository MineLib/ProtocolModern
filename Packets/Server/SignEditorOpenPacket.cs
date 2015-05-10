using MineLib.Core;
using MineLib.Core.Data;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct SignEditorOpenPacket : IPacket
    {
        public Position Location;

        public byte ID { get { return 0x36; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromReaderLong(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            
            return this;
        }
    }
}