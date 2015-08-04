using System;

namespace ProtocolModern
{
    // https://github.com/sinhpham/MurmurHash3/blob/1f81df67703d70746f30d065781e9d6f2b33bf49/MurmurHash3/MurmurHash3_x86_32.cs
    public interface IHashFunc
    {
        byte[] ComputeHash(byte[] buffer);
        byte[] ComputeHash(byte[] buffer, int offset, int count);
    }

    public sealed class MurmurHash3_32 : IHashFunc
    {
        public uint Seed { get; set; }

        private uint _h1 = 0;

        private void HashCore(byte[] array, int ibStart, int cbSize)
        {
            byte[] data = array;
            var len = cbSize;
            var nblocks = len / 4;

            _h1 = Seed;

            const UInt32 c1 = 0xcc9e2d51;
            const UInt32 c2 = 0x1b873593;

            //----------
            // body
            UInt32 k1 = 0;
            for (int i = 0; i < nblocks; ++i)
            {
                k1 = BitConverter.ToUInt32(data, ibStart + i * 4);

                k1 *= c1;
                k1 = RotateLeft32(k1, 15);
                k1 *= c2;

                _h1 ^= k1;
                _h1 = RotateLeft32(_h1, 13);
                _h1 = _h1 * 5 + 0xe6546b64;
            }

            //----------
            // tail
            k1 = 0;
            var tailIdx = ibStart + nblocks * 4;
            switch (len & 3)
            {
                case 3:
                    k1 ^= (UInt32)(data[tailIdx + 2]) << 16;
                    goto case 2;
                case 2:
                    k1 ^= (UInt32)(data[tailIdx + 1]) << 8;
                    goto case 1;
                case 1:
                    k1 ^= (UInt32)(data[tailIdx + 0]);
                    k1 *= c1; k1 = RotateLeft32(k1, 15); k1 *= c2; _h1 ^= k1;
                    break;
            };

            //----------
            // finalization
            _h1 ^= (UInt32)len;
            _h1 = FMix(_h1);
        }

        uint RotateLeft32(uint x, byte r)
        {
            return (x << r) | (x >> (32 - r));
        }

        uint FMix(uint h)
        {
            h ^= h >> 16;
            h *= 0x85ebca6b;
            h ^= h >> 13;
            h *= 0xc2b2ae35;
            h ^= h >> 16;

            return h;
        }

        private byte[] HashFinal()
        {
            var hashVal = new byte[4];
            Array.Copy(BitConverter.GetBytes(_h1), 0, hashVal, 0, 4);
            return hashVal;
        }

        public byte[] ComputeHash(byte[] buffer)
        {
            return ComputeHash(buffer, 0, buffer.Length);
        }


        public byte[] ComputeHash(byte[] buffer, int offset, int count)
        {
            HashCore(buffer, 0, count);
            return HashFinal();
        }
    }
}
