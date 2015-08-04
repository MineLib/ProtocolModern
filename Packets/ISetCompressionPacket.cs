using MineLib.Core.Interfaces;

namespace ProtocolModern.Packets
{
    public interface ISetCompressionPacket : IPacket
    {
        int Threshold { get; set; }
    }
}