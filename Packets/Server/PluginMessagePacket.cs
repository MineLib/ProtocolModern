using MineLib.Core;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel;
        public byte[] Data;

        public byte ID { get { return 0x3F; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Channel = reader.ReadString();
            if (Channel == "REGISTER" || Channel == "MC|RPack")
                return this;

            var length = reader.ReadVarInt();
            Data = reader.ReadByteArray(length);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteVarInt(ID);
            stream.WriteString(Channel);
            stream.WriteVarInt(Data.Length);
            stream.WriteByteArray(Data);
            
            return this;
        }
    }
}