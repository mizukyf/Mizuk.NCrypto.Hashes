using System;

namespace Mizuk.NCrypto.Hashes.Util
{
    /// <summary>
    /// This code is derived from  RustCrypto/hashes and Rust std modules.
    /// Ported by mizuky at 2020/09/01.
    /// </summary>
    static class NCryptoHashesExtension
    {
        public static uint RotateLeft(this uint v, int n)
        {
            return (v << n) | (v >> (32 - n));
        }
        public static uint FromLittleEndianBytes(this byte[] bytes)
        {
            if (bytes.Length != 4) throw new ArgumentException("bytes' length must be 4.");
            if (!BitConverter.IsLittleEndian)
            {
                var tmp = new byte[4];
                bytes.CopyTo(tmp, 0);
                Array.Reverse(tmp);
                bytes = tmp;
            }
            return BitConverter.ToUInt32(bytes, 0);
        }
        public static byte[] ToLittleEndianBytes(this ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
        public static byte[] ToLittleEndianBytes(this uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
        public static ChunksExact ChunksExact(this byte[] values, int chunkSize)
        {
            return new ChunksExact(values, chunkSize);
        }
    }
}
