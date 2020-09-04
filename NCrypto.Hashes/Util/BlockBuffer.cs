using NCrypto.Hashes.Traits;
using System;
using System.Linq;

namespace NCrypto.Hashes.Util
{
    /// <summary>
    /// バイト列からなるデータをブロックごとに処理するためのバッファーです。
    /// RustCrypto/hasesのRustコードをC#コードへとポーティングする際に必要となった最小限の機能のみ有します。
    /// </summary>
    /// <remarks>
    /// This code is derived from  "block_buffer::BlockBuffer" in Rust std modules.
    /// Ported by mizuky at 2020/09/01.
    /// </remarks>
    sealed class BlockBuffer : IClone<BlockBuffer>
    {
        readonly byte[] _buffer;
        int _pos;

        internal BlockBuffer(int blockSize)
        {
            if (blockSize < 1) throw new ArgumentException("blockSize must be greater than 0.");
            Size = blockSize;
            _buffer = new byte[blockSize];
        }

        /// <summary>
        /// このバッファーのサイズです。
        /// </summary>
        public int Size { get; private set; }
        /// <summary>
        /// バッファーに現在残されている空きスペースのサイズです。
        /// </summary>
        public int Remaining { get { return Size - _pos; } }

        public BlockBuffer Clone()
        {
            var clone = new BlockBuffer(Size);
            _buffer.CopyTo(clone._buffer, 0);
            clone._pos = _pos;
            return clone;
        }

        /// <summary>
        /// 入力となるバイト列をバッファに格納します。
        /// バッファが満たされた時点で都度、指定されたアクションを実行してバッファをクリアします。
        /// 
        /// 入力となるデータのバイト列サイズがバッファの現在の<see cref="Remaining"/>未満の場合、
        /// 入力データはバッファに格納され、アクションは実行されません。
        /// 入力となるデータのバイト列サイズがバッファの現在の<see cref="Remaining"/>以上の場合、
        /// そのサイズに応じてバッファの充足、アクションの実行、バッファのクリアという一連の動作が繰り返されます。
        /// アクション<paramref name="f"/>に渡されるバイト列のサイズはバッファのサイズと一致します。
        /// </summary>
        /// <param name="input">入力データのバイト列</param>
        /// <param name="f">バッファが充足されるたび実行されるアクション</param>
        public void InputBlock(byte[] input, Action<byte[]> f)
        {
            var r = Remaining;
            if (input.Length < r)
            {
                var n = input.Length;
                input.CopyTo(_buffer, _pos);
                _pos += n;
                return;
            }
            if (_pos != 0 && input.Length >= r)
            {
                var left = input.Take(r).ToArray();
                var right = input.Skip(r).ToArray();
                input = right;
                left.CopyTo(_buffer, _pos);
                f(left);
            }

            var chunks = input.ChunksExact(Size);
            foreach(var chunk in chunks)
            {
                f(chunk);
            }

            var rem = chunks.GetRemainder();
            rem.CopyTo(_buffer, 0);
            _pos = rem.Length;
        }

        /// <summary>
        /// メッセージを、<c>0x80</c>、それに続くゼロの羅列、そして64bitのメッセージ長値を
        /// リトルエンディアンでバイト列にしたものでパディングします。
        /// </summary>
        /// <param name="dataLength">メッセージ長</param>
        /// <param name="f">バッファに所定の空きがないとき実行されるアクション</param>
        public void Length64PaddingLittleEndian(ulong dataLength, Action<byte[]> f)
        {
            DigestPadding(8, f);
            var b = dataLength.ToLittleEndianBytes();
            var n = _buffer.Length - b.Length;
            b.CopyTo(_buffer, n);
            f(_buffer);
            _pos = 0;
        }

        /// <summary>
        /// バッファをリセットします。
        /// </summary>
        public void Reset()
        {
            _pos = 0;
        }

        /// <summary>
        /// 接頭辞（<c>0x80</c>）とそれに続くゼロの羅列でバッファにパディングを行います。
        /// 加えて、<paramref name="upTo"/>で指定されただけの空きスペースが確保されるようにします。
        /// バッファ内のバイト列の残りの部分はすべてゼロで埋められます。
        /// </summary>
        /// <param name="upTo">最低限確保すべきバッファの空きスペース</param>
        /// <param name="f">バッファに所定の空きがないとき実行されるアクション</param>
        void DigestPadding(int upTo, Action<byte[]> f)
        {
            if (_pos == Size)
            {
                f(_buffer);
                _pos = 0;
            }
            _buffer[_pos] = 0x80;
            _pos += 1;

            SetZero(_pos, _buffer.Length);

            if (Remaining < upTo)
            {
                f(_buffer);
                SetZero(0, _pos);
            }
        }

        void SetZero(int startIndex, int endIndexExclusive)
        {
            for (var i = startIndex; i < endIndexExclusive; i++)
            {
                _buffer[i] = 0;
            }
        }
    }
}
