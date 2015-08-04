using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Security;

namespace ProtocolModern
{
    public sealed class PKCS1Signature
    {
        // Not really related to this class.
        public static byte[] CreateSecretKey(int length = 16)
        {
            var generator = new CipherKeyGenerator();
            generator.Init(new KeyGenerationParameters(new SecureRandom(), length * 8));

            return generator.GenerateKey();
        }


        private readonly AsymmetricKeyParameter _publicKey;
        public PKCS1Signature(byte[] publicKey)
        {
            _publicKey = PublicKeyFactory.CreateKey(publicKey);
        }

        public byte[] SignData(byte[] data)
        {
            var eng = new Pkcs1Encoding(new RsaEngine());
            eng.Init(true, _publicKey);
            return eng.ProcessBlock(data, 0, data.Length);
        }
    }
}
