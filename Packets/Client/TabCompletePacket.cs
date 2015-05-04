using MineLib.Network;
using MineLib.Network.Data;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct TabCompletePacket : IPacket
    {
        public string Text;
        public bool HasPosition;
        public Position LookedAtBlock;

        public byte ID { get { return 0x14; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Text = reader.ReadString();
            HasPosition = reader.ReadBoolean();
            LookedAtBlock = Position.FromReaderLong(reader);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Text);
            stream.WriteBoolean(HasPosition);
            LookedAtBlock.ToStreamLong(stream);
            stream.Purge();

            return this;
        }
    }
}