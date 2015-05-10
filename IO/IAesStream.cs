using System;
using System.Threading.Tasks;

namespace ProtocolModern.IO
{
    public interface IAesStreamAsync
    {
        Task<Int32> ReadAsync(byte[] buffer, int offset, int count);

        Task WriteAsync(byte[] buffer, int offset, int count);
    }

    /// <summary>
    /// Object that implements AES.
    /// </summary>
    public interface IAesStream: IAesStreamAsync, IDisposable
    {
        int ReadByte();
        int Read(byte[] buffer, int offset, int count);

        void WriteByte(byte value);
        void Write(byte[] buffer, int offset, int count);
    }
}