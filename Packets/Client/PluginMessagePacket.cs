using System.Text;

using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Client
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }
        public string InString { get; set; }

        public byte ID { get { return 0x17; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Channel = reader.ReadString();

            var length = reader.BytesLeft();
            Data = reader.ReadByteArray(length);
            InString = Encoding.UTF8.GetString(Data, 0, Data.Length);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteString(Channel);
            stream.WriteByteArray(Data);
            
            return this;
        }
    }
}