using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Server.Login
{
    public interface ISetCompression : IPacket
    {
        int Threshold { get; set; }
    }

    public struct SetCompressionPacket : ISetCompression
    {
        public int Threshold { get; set; }

        public byte ID { get { return 0x03; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Threshold = reader.ReadVarInt();

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteVarInt(Threshold);
            stream.Purge();

            return this;
        }
    }
}
