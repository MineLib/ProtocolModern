using MineLib.Network;
using MineLib.Network.IO;
using ProtocolModern.Packets.Server.Login;

namespace ProtocolModern.Packets.Server
{
    public struct SetCompressionPacket : ISetCompression
    {
        public int Threshold { get; set; }

        public byte ID { get { return 0x46; } }

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
