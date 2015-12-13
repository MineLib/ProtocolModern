using System;
using System.IO;

using Aragas.Core.Data;
using Aragas.Core.IO;
using Aragas.Core.Wrappers;

using ICSharpCode.SharpZipLib.Zip.Compression;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;

namespace ProtocolModern.IO
{
    public sealed class CompressionProtobufStream : ProtobufStream
    {
        public bool CompressionEnabled { get; private set; }
        public long CompressionThreshold { get; private set; }

        public CompressionProtobufStream(ITCPClient tcp) : base(tcp, false) { }

        public void SetCompression(long threshold)
        {
            if (threshold == -1)
            {
                CompressionEnabled = false;
                CompressionThreshold = 0;
            }

            CompressionEnabled = true;
            CompressionThreshold = threshold;
        }

        protected override void Purge()
        {
            if (CompressionEnabled)
                PurgeModernWithCompression();
            else
                PurgeModernWithoutCompression();
        }
        private void PurgeModernWithoutCompression()
        {
            var lenBytes = new VarInt(_buffer.Length).InByteArray();

            var tempBuff = new byte[_buffer.Length + lenBytes.Length];

            Buffer.BlockCopy(lenBytes, 0, tempBuff, 0, lenBytes.Length);
            Buffer.BlockCopy(_buffer, 0, tempBuff, lenBytes.Length, _buffer.Length);

            Send(tempBuff);

            _buffer = null;
        }
        private void PurgeModernWithCompression()
        {
            int packetLength = 0; // -- data.Length + GetVarIntBytes(data.Length).Length
            int dataLength = 0; // -- UncompressedData.Length
            var data = _buffer;

            packetLength = _buffer.Length + new VarInt(_buffer.Length).InByteArray().Length; // -- Get first Packet length

            if (packetLength >= CompressionThreshold) // -- if Packet length > threshold, compress
            {
                using (var outputStream = new MemoryStream())
                using (var inputStream = new DeflaterOutputStream(outputStream, new Deflater(0)))
                {
                    inputStream.Write(_buffer, 0, _buffer.Length);
                    inputStream.Finish();

                    data = outputStream.ToArray();
                }

                dataLength = data.Length;
                packetLength = dataLength + new VarInt(data.Length).InByteArray().Length; // -- Calculate new packet length
            }


            var packetLengthByteLength = new VarInt(packetLength).InByteArray();
            var dataLengthByteLength = new VarInt(dataLength).InByteArray();

            var tempBuff = new byte[data.Length + packetLengthByteLength.Length + dataLengthByteLength.Length];

            Buffer.BlockCopy(packetLengthByteLength, 0, tempBuff, 0, packetLengthByteLength.Length);
            Buffer.BlockCopy(dataLengthByteLength, 0, tempBuff, packetLengthByteLength.Length, dataLengthByteLength.Length);
            Buffer.BlockCopy(data, 0, tempBuff, packetLengthByteLength.Length + dataLengthByteLength.Length, data.Length);

            Send(tempBuff);

            _buffer = null;
        }
    }
}