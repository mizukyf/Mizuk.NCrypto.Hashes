using NCrypto.Hashes.Traits;
using NCrypto.Hashes.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCrypto.Hashes
{
    /// <summary>
    /// MD5ハッシュの機能を提供するクラスです。
    /// </summary>
    /// <remarks>
    /// This code is derived from  RustCrypto/hashes.
    /// Ported by mizuky at 2020/09/04.
    /// </remarks>
    public sealed class Md5 : IFixedOutput, IFixedOutputDirty, IReset, IBlockInput, IClone<Md5>, IUpdate<Md5>, IDigest<Md5>
    {
        #region consts.rs

        /// <summary>
        /// Round constants
        /// </summary>
        static readonly uint[] _RountConstants =
        {
            // round 1
            0xd76aa478, 0xe8c7b756, 0x242070db, 0xc1bdceee, 0xf57c0faf, 0x4787c62a, 0xa8304613, 0xfd469501,
            0x698098d8, 0x8b44f7af, 0xffff5bb1, 0x895cd7be, 0x6b901122, 0xfd987193, 0xa679438e, 0x49b40821,
            // round 2
            0xf61e2562, 0xc040b340, 0x265e5a51, 0xe9b6c7aa, 0xd62f105d, 0x02441453, 0xd8a1e681, 0xe7d3fbc8,
            0x21e1cde6, 0xc33707d6, 0xf4d50d87, 0x455a14ed, 0xa9e3e905, 0xfcefa3f8, 0x676f02d9, 0x8d2a4c8a,
            // round 3
            0xfffa3942, 0x8771f681, 0x6d9d6122, 0xfde5380c, 0xa4beea44, 0x4bdecfa9, 0xf6bb4b60, 0xbebfbc70,
            0x289b7ec6, 0xeaa127fa, 0xd4ef3085, 0x04881d05, 0xd9d4d039, 0xe6db99e5, 0x1fa27cf8, 0xc4ac5665,
            // round 4
            0xf4292244, 0x432aff97, 0xab9423a7, 0xfc93a039, 0x655b59c3, 0x8f0ccc92, 0xffeff47d, 0x85845dd1,
            0x6fa87e4f, 0xfe2ce6e0, 0xa3014314, 0x4e0811a1, 0xf7537e82, 0xbd3af235, 0x2ad7d2bb, 0xeb86d391
        };

        /// <summary>
        /// Init state
        /// </summary>
        static readonly uint[] _InitState = { 0x67452301, 0xefcdab89, 0x98badcfe, 0x10325476 };

        #endregion consts.rs

        static internal readonly int _BlockSize = 64;
        static internal readonly int _OutputSize = 16;

        ulong LengthBytes;
        BlockBuffer Buffer;
        uint[] State;

        /// <summary>
        /// 出力されるバイト列のサイズです。
        /// </summary>
        public int OutputSize
        {
            get
            {
                return _OutputSize;
            }
        }
        /// <summary>
        /// ブロックのサイズです。
        /// </summary>
        public int BlockSize
        {
            get
            {
                return _BlockSize;
            }
        }

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public Md5()
        {
            LengthBytes = 0;
            Buffer = new BlockBuffer(_BlockSize);
            _InitState.CopyTo(State, 0);
        }
        void FinalizeInner()
        {
            var l = LengthBytes << 3;
            Buffer.Length64PaddingLittleEndian(l, d => Compress(State, Convert(d)));
        }

        /// <summary>
        /// 入力データのバイト列を利用してMD5メッセージダイジェストの計算を続けます。
        /// </summary>
        /// <param name="input"></param>
        public void Update(byte[] input)
        {
            LengthBytes += (ulong)input.Length;
            Buffer.InputBlock(input, d => Compress(State, Convert(d)));
        }

        /// <summary>
        /// MD5メッセージダイジェストの計算結果を引数で指定されたバッファに回収します。
        /// そしてオブジェクトの内部状態はダーティなままにします。
        /// </summary>
        /// <param name="output"></param>
        public void FinalizeIntoDirty(byte[] output)
        {
            FinalizeInner();

            foreach (var x in Enumerable.Range(0, output.Length).Where(x => x % 4 == 0)
                .Zip(State, (a, b) => new { StartIndex = a, Value = b }))
            {
                x.Value.ToLittleEndianBytes().CopyTo(output, x.StartIndex);
            }
        }

        /// <summary>
        /// このオブジェクトの内部状態をリセットします。
        /// </summary>
        public void Reset()
        {
            LengthBytes = 0;
            Buffer.Reset();
            _InitState.CopyTo(State, 0);
        }

        /// <summary>
        /// このオブジェクトとその内部状態をクローンします。
        /// </summary>
        /// <returns></returns>
        public Md5 Clone()
        {
            var clone = new Md5();
            State.CopyTo(clone.State, 0);
            clone.LengthBytes = LengthBytes;
            clone.Buffer = Buffer.Clone();
            return clone;
        }

        /// <summary>
        /// 引数指定されたバッファに結果を回収します。
        /// </summary>
        /// <param name="output"></param>
        public void FinalizeInto(byte[] output)
        {
            FixedOutputDirtyImpl.FinalizeInto(this, output);
        }

        /// <summary>
        /// 引数指定されたバッファに結果を回収、その後内部状態をリセットします。
        /// </summary>
        /// <param name="output"></param>
        public void FinalizeIntoReset(byte[] output)
        {
            FixedOutputDirtyImpl.FinalizeIntoReset(this, output);
        }

        /// <summary>
        /// 結果を取得します。
        /// </summary>
        /// <returns></returns>
        public byte[] FinalizeFixed()
        {
            return FixedOutputImpl.FinalizeFixed(this);
        }

        /// <summary>
        /// 結果を取得し内部状態をリセットします。
        /// </summary>
        /// <returns></returns>
        public byte[] FinalizeFixedReset()
        {
            return FixedOutputImpl.FinalizeFixedReset(this);
        }

        /// <summary>
        /// メソッドチェインの形式でデータを変換します。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Md5 Chain(byte[] data)
        {
            return UpdateImpl.Chain(this, data);
        }

        /// <summary>
        /// 結果を取得します。
        /// </summary>
        /// <returns></returns>
        public byte[] Finalize()
        {
            return DigestImpl.Finalize(this);
        }

        /// <summary>
        /// 結果を取得し、内部状態をリセットします。
        /// このメソッドはオブジェクトを再作成するのに比べて効率的なことがあります。
        /// </summary>
        /// <returns></returns>
        public byte[] FinalizeReset()
        {
            return DigestImpl.FinalizeReset(this);
        }
        /// <summary>
        /// 指定されたバイト配列をMD5ダイジェストメッセージに変換します。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Digest(IEnumerable<byte> bytes)
        {
            var md5 = new Md5();
            md5.Update(bytes.ToArray());
            return md5.FinalizeFixed();
        }
        /// <summary>
        /// 指定された文字列をUTF16のバイト配列にした上でMD5ダイジェストメッセージに変換します。
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static byte[] Digest(IEnumerable<char> chars)
        {
            return Digest(Encoding.Unicode.GetBytes(chars.ToArray()));
        }

        #region utils
        byte[] Convert(byte[] d)
        {
            return d; // ?
        }
        void Compress(uint[] s, byte[] input)
        {
            var a = State[0];
            var b = State[1];
            var c = State[2];
            var d = State[3];

            var data = new uint[16];
            foreach (var x in input.ChunksExact(4).Select((e, i) => new { DataIndex = i, InputChunk = e }))
            {
                data[x.DataIndex] = x.InputChunk.FromLittleEndianBytes();
            }

            // round 1
            a = op_f(a, b, c, d, data[0], _RountConstants[0], 7);
            d = op_f(d, a, b, c, data[1], _RountConstants[1], 12);
            c = op_f(c, d, a, b, data[2], _RountConstants[2], 17);
            b = op_f(b, c, d, a, data[3], _RountConstants[3], 22);

            a = op_f(a, b, c, d, data[4], _RountConstants[4], 7);
            d = op_f(d, a, b, c, data[5], _RountConstants[5], 12);
            c = op_f(c, d, a, b, data[6], _RountConstants[6], 17);
            b = op_f(b, c, d, a, data[7], _RountConstants[7], 22);

            a = op_f(a, b, c, d, data[8], _RountConstants[8], 7);
            d = op_f(d, a, b, c, data[9], _RountConstants[9], 12);
            c = op_f(c, d, a, b, data[10], _RountConstants[10], 17);
            b = op_f(b, c, d, a, data[11], _RountConstants[11], 22);

            a = op_f(a, b, c, d, data[12], _RountConstants[12], 7);
            d = op_f(d, a, b, c, data[13], _RountConstants[13], 12);
            c = op_f(c, d, a, b, data[14], _RountConstants[14], 17);
            b = op_f(b, c, d, a, data[15], _RountConstants[15], 22);

            // round 2
            a = op_g(a, b, c, d, data[1], _RountConstants[16], 5);
            d = op_g(d, a, b, c, data[6], _RountConstants[17], 9);
            c = op_g(c, d, a, b, data[11], _RountConstants[18], 14);
            b = op_g(b, c, d, a, data[0], _RountConstants[19], 20);

            a = op_g(a, b, c, d, data[5], _RountConstants[20], 5);
            d = op_g(d, a, b, c, data[10], _RountConstants[21], 9);
            c = op_g(c, d, a, b, data[15], _RountConstants[22], 14);
            b = op_g(b, c, d, a, data[4], _RountConstants[23], 20);

            a = op_g(a, b, c, d, data[9], _RountConstants[24], 5);
            d = op_g(d, a, b, c, data[14], _RountConstants[25], 9);
            c = op_g(c, d, a, b, data[3], _RountConstants[26], 14);
            b = op_g(b, c, d, a, data[8], _RountConstants[27], 20);

            a = op_g(a, b, c, d, data[13], _RountConstants[28], 5);
            d = op_g(d, a, b, c, data[2], _RountConstants[29], 9);
            c = op_g(c, d, a, b, data[7], _RountConstants[30], 14);
            b = op_g(b, c, d, a, data[12], _RountConstants[31], 20);

            // round 3
            a = op_h(a, b, c, d, data[5], _RountConstants[32], 4);
            d = op_h(d, a, b, c, data[8], _RountConstants[33], 11);
            c = op_h(c, d, a, b, data[11], _RountConstants[34], 16);
            b = op_h(b, c, d, a, data[14], _RountConstants[35], 23);

            a = op_h(a, b, c, d, data[1], _RountConstants[36], 4);
            d = op_h(d, a, b, c, data[4], _RountConstants[37], 11);
            c = op_h(c, d, a, b, data[7], _RountConstants[38], 16);
            b = op_h(b, c, d, a, data[10], _RountConstants[39], 23);

            a = op_h(a, b, c, d, data[13], _RountConstants[40], 4);
            d = op_h(d, a, b, c, data[0], _RountConstants[41], 11);
            c = op_h(c, d, a, b, data[3], _RountConstants[42], 16);
            b = op_h(b, c, d, a, data[6], _RountConstants[43], 23);

            a = op_h(a, b, c, d, data[9], _RountConstants[44], 4);
            d = op_h(d, a, b, c, data[12], _RountConstants[45], 11);
            c = op_h(c, d, a, b, data[15], _RountConstants[46], 16);
            b = op_h(b, c, d, a, data[2], _RountConstants[47], 23);

            // round 4
            a = op_i(a, b, c, d, data[0], _RountConstants[48], 6);
            d = op_i(d, a, b, c, data[7], _RountConstants[49], 10);
            c = op_i(c, d, a, b, data[14], _RountConstants[50], 15);
            b = op_i(b, c, d, a, data[5], _RountConstants[51], 21);

            a = op_i(a, b, c, d, data[12], _RountConstants[52], 6);
            d = op_i(d, a, b, c, data[3], _RountConstants[53], 10);
            c = op_i(c, d, a, b, data[10], _RountConstants[54], 15);
            b = op_i(b, c, d, a, data[1], _RountConstants[55], 21);

            a = op_i(a, b, c, d, data[8], _RountConstants[56], 6);
            d = op_i(d, a, b, c, data[15], _RountConstants[57], 10);
            c = op_i(c, d, a, b, data[6], _RountConstants[58], 15);
            b = op_i(b, c, d, a, data[13], _RountConstants[59], 21);

            a = op_i(a, b, c, d, data[4], _RountConstants[60], 6);
            d = op_i(d, a, b, c, data[11], _RountConstants[61], 10);
            c = op_i(c, d, a, b, data[2], _RountConstants[62], 15);
            b = op_i(b, c, d, a, data[9], _RountConstants[63], 21);

            State[0] += a;
            State[1] += b;
            State[2] += c;
            State[3] += d;
        }

        uint op_f(uint w, uint x, uint y, uint z, uint m, uint c, int s)
        {
            return (((x & y) | (~x & z)) + w + m + c).RotateLeft(s) + x;
        }
        uint op_g(uint w, uint x, uint y, uint z, uint m, uint c, int s)
        {
            return (((x & z) | (y & ~z)) + w + m + c).RotateLeft(s) + x;
        }
        uint op_h(uint w, uint x, uint y, uint z, uint m, uint c, int s)
        {
            return ((x ^ y ^ z) + w + m + c).RotateLeft(s) + x;
        }
        uint op_i(uint w, uint x, uint y, uint z, uint m, uint c, int s)
        {
            return ((y ^ (x | ~z)) + w + m + c).RotateLeft(s) + x;
        }

        #endregion utils

    }
}
