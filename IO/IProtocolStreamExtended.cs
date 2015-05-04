using MineLib.Network.IO;

namespace ProtocolModern.IO
{
    public interface IProtocolStreamExtended : IProtocolStream
    {
        bool EncryptionEnabled { get; }

        bool ModernCompressionEnabled { get; }
        long ModernCompressionThreshold { get; }


        void InitializeEncryption(byte[] key);

        void SetCompression(long threshold);
    }
}