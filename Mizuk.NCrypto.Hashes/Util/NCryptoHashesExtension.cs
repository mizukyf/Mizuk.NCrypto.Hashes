using System;

namespace Mizuk.NCrypto.Hashes.Util
{
    /// <summary>
    /// This code is derived from  RustCrypto/hashes and Rust std modules.
    /// Ported by mizuky at 2020/09/01.
    /// 
    /// Mizuk.NCrypto.Hashes内のコードのため、RustからC#へのポーティングをできるだけシンプルに行うための
    /// 各種の拡張メソッドを提供するユーティリティクラスです。
    /// </summary>
    static class NCryptoHashesExtension
    {
        /// <summary>
        /// ビットを左方向にシフト、ローテートを行います。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="n"></param>
        /// <returns></returns>
        public static uint RotateLeft(this uint value, int n)
        {
            return (value << n) | (value >> (32 - n));
        }
        /// <summary>
        /// 4要素からなるバイト配列を元にリトルエンディアンで整数値に変換します。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
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
        /// <summary>
        /// 整数値をリトルエンディアンのバイト配列に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToLittleEndianBytes(this ulong value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
        /// <summary>
        /// 整数値をリトルエンディアンのバイト配列に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToLittleEndianBytes(this uint value)
        {
            var bytes = BitConverter.GetBytes(value);
            if (!BitConverter.IsLittleEndian)
            {
                Array.Reverse(bytes);
            }
            return bytes;
        }
        /// <summary>
        /// バイト配列を指定されたチャンクサイズに対応する<see cref="ChunksExact"/>インスタンスを返します。
        /// </summary>
        /// <param name="values"></param>
        /// <param name="chunkSize"></param>
        /// <returns></returns>
        public static ChunksExact ChunksExact(this byte[] values, int chunkSize)
        {
            return new ChunksExact(values, chunkSize);
        }
    }
}
