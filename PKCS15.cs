using System.Collections.Generic;
using System.Text;

using MineLib.Core;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;


namespace ProtocolModern
{
    public static class PKCS15
    {
        public static string GetServerIDHash(byte[] publicKey, byte[] secretKey, string serverID)
        {
            var hashlist = new List<byte>();
            hashlist.AddRange(Encoding.UTF8.GetBytes(serverID));
            hashlist.AddRange(secretKey);
            hashlist.AddRange(publicKey);

            return JavaHelper.JavaHexDigest(hashlist.ToArray());
        }


        public static byte[] CreateSecretKey(int length = 16)
        {
            //var random = new Random();
            //var privateKey = new byte[length];
            //random.NextBytes(privateKey);
            //
            //return privateKey;

            var generator = new CipherKeyGenerator();
            generator.Init(new KeyGenerationParameters(new SecureRandom(), length * 8));
            
            return generator.GenerateKey();
        }

        
        public static RsaKeyParameters GetRsaKeyParameters(byte[] publicKey)
        {
            var kp = PublicKeyFactory.CreateKey(publicKey);
            var rsaKeyParameters = kp as RsaKeyParameters;

            return rsaKeyParameters;
        }

        public static byte[] EncryptData(RsaKeyParameters rsaParameters, byte[] data)
        {
            var eng = new Pkcs1Encoding(new RsaEngine());
            eng.Init(true, rsaParameters);
            return eng.ProcessBlock(data, 0, data.Length);
        }

        public static byte[] DecryptData(RsaKeyParameters rsaParameters, byte[] data)
        {
            var eng = new Pkcs1Encoding(new RsaEngine());
            eng.Init(false, rsaParameters);
            return eng.ProcessBlock(data, 0, data.Length);
        }
    }
}
