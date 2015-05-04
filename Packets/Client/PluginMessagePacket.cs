using MineLib.Network;
using MineLib.Network.IO;

namespace ProtocolModern.Packets.Client
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel;
        public byte[] Data;

        public byte ID { get { return 0x17; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Channel = reader.ReadString();
            int length = reader.ReadShort();
            Data = reader.ReadByteArray(length);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Channel);
            stream.WriteShort((short) Data.Length);
            stream.WriteByteArray(Data);
            stream.Purge();

            return this;
        }
    }
}