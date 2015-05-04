using System;

using MineLib.Network.IO;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;

namespace ProtocolModern.IO
{
    public sealed class BouncyCastleAES : IAesStream
    {
        private readonly INetworkTCP _tcp;

        private readonly BufferedBlockCipher _decryptCipher;
        private readonly BufferedBlockCipher _encryptCipher;

        public BouncyCastleAES(INetworkTCP tcp, byte[] key)
        {
            _tcp = tcp;

            _encryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _encryptCipher.Init(true, new ParametersWithIV(new KeyParameter(key), key, 0, 16));

            _decryptCipher = new BufferedBlockCipher(new CfbBlockCipher(new AesFastEngine(), 8));
            _decryptCipher.Init(false, new ParametersWithIV(new KeyParameter(key), key, 0, 16));
        }


        public int ReadByte()
        {
            var buff = new byte[1];
            _tcp.Receive(buff, 0, buff.Length);

            if (buff[0] == -1)
                return buff[0];
            else
                return _decryptCipher.ProcessByte(buff[0])[0];
        }

        public int Read(byte[] buffer, int offset, int count)
        {
            var length = _tcp.Receive(buffer, offset, count);
            var decrypted = _decryptCipher.ProcessBytes(buffer, offset, length);
            Buffer.BlockCopy(decrypted, 0, buffer, offset, decrypted.Length);
            return length;
        }


        public void WriteByte(byte value)
        {
            var encrypted = _encryptCipher.ProcessBytes(new byte[] { value }, 0, 1);
            _tcp.Send(encrypted, 0, encrypted.Length);  
        }

        public void Write(byte[] buffer, int offset, int count)
        {
            var encrypted = _encryptCipher.ProcessBytes(buffer, offset, count);
            _tcp.Send(encrypted, 0, encrypted.Length);
        }


        public IAsyncResult BeginRead(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            return _tcp.BeginReceive(buffer, offset, count, callback, state);
        }
        public int EndRead(IAsyncResult asyncResult)
        {
            return _tcp.EndReceive(asyncResult);
        }


        public IAsyncResult BeginWrite(byte[] buffer, int offset, int count, AsyncCallback callback, object state)
        {
            var encrypted = _encryptCipher.ProcessBytes(buffer, offset, count);
            return _tcp.BeginSend(encrypted, 0, encrypted.Length, callback, state);
        }
        public void EndWrite(IAsyncResult asyncResult)
        {
            _tcp.EndSend(asyncResult);
        }

        
        public void Dispose()
        {
            if (_decryptCipher != null)
                _decryptCipher.Reset();

            if (_encryptCipher != null)
                _encryptCipher.Reset();

            if (_tcp != null)
                _tcp.Dispose();
        }
    }
}
