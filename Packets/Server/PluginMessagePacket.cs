using System.Text;

using MineLib.Core.Interfaces;
using MineLib.Core.IO;

namespace ProtocolModern.Packets.Server
{
    public struct PluginMessagePacket : IPacket
    {
        public string Channel { get; set; }
        public byte[] Data { get; set; }
        public string InString { get; set; }

        public byte ID { get { return 0x3F; } }

        public IPacket ReadPacket(IProtocolDataReader reader)
        {
            Channel = reader.ReadString(20);

            var length = reader.BytesLeft();
            Data = reader.ReadByteArray(length);
            InString = Encoding.UTF8.GetString(Data, 0, Data.Length);

            return this;
        }

        public IPacket WritePacket(IProtocolStream stream)
        {
            stream.WriteString(Channel, 20);
            stream.WriteByteArray(Data);
            
            return this;
        }
    }
}