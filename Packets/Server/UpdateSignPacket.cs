using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server
{
    public struct UpdateSignPacket : IPacket
    {
        public Position Location;
        public string[] Text;

        public byte ID { get { return 0x33; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Location = Position.FromReaderLong(reader);
            Text = new string[4];
            Text[0] = reader.ReadString();
            Text[1] = reader.ReadString();
            Text[2] = reader.ReadString();
            Text[3] = reader.ReadString();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            Location.ToStreamLong(stream);
            stream.WriteString(Text[0]);
            stream.WriteString(Text[1]);
            stream.WriteString(Text[2]);
            stream.WriteString(Text[3]);
            stream.Purge();

            return this;
        }
    }
}