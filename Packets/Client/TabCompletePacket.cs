using MineLib.Core.Data;
using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct TabCompletePacket : IPacket
    {
        public string Text { get; set; }
        public bool HasPosition { get; set; }
        public Position LookedAtBlock { get; set; }

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
            stream.WriteString(Text);
            stream.WriteBoolean(HasPosition);
            LookedAtBlock.ToStreamLong(stream);
            
            return this;
        }
    }
}