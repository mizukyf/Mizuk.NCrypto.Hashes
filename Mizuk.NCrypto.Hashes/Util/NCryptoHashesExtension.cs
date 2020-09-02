using System;
using System.Linq;

namespace Mizuk.NCrypto.Hashes.Util
{
    /// <summary>
    /// Mizuk.NCrypto.Hashes内のコードのため、RustからC#へのポーティングをできるだけシンプルに行うための
    /// 各種の拡張メソッドを提供するユーティリティクラスです。
    /// </summary>
    /// <remarks>
    /// This code is derived from  RustCrypto/hashes and Rust std modules.
    /// Ported by mizuky at 2020/09/01.
    /// 
    /// </remarks>
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
        /// バイト配列の要素数が5以上であった場合それら後続の値は単に無視されます。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">バイト配列の要素数が4未満である場合</exception>
        public static uint FromLittleEndianBytes(this byte[] bytes)
        {
            return FromLittleEndianBytes(bytes, 0);
        }
        /// <summary>
        /// バイト配列の指定された位置から始まる4要素を元にリトルエンディアンで整数値に変換します。
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="startIndex"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">バイト配列の指定された位置以降の要素数が4未満である場合</exception>
        public static uint FromLittleEndianBytes(this byte[] bytes, int startIndex)
        {
            if (bytes.Length - startIndex < 4)
            {
                throw new ArgumentException("not enough values to convert. "
                    + string.Format("bytes.Length = {0}, startIndex = {1}.",
                    bytes.Length, startIndex));
            }
            return (uint)Enumerable.Range(0, 4)
                .Select(i => (bytes[i] & 0xFF) << (8 * i))
                .Aggregate((a, b) => a | b);
        }
        /// <summary>
        /// 整数値をリトルエンディアンのバイト配列に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToLittleEndianBytes(this ulong value)
        {
            return Enumerable.Range(0, 8)
                .Select(i => (byte)((value >> (8 * i)) & 0xFF))
                .ToArray(); ;
        }
        /// <summary>
        /// 整数値をリトルエンディアンのバイト配列に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static byte[] ToLittleEndianBytes(this uint value)
        {
            return Enumerable.Range(0, 4)
                .Select(i => (byte)((value >> (8 * i)) & 0xFF))
                .ToArray(); ;
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
