using System;
using System.Linq;

namespace Mizuk.NCrypto.Hashes.Util
{
    /// <summary>
    /// This code is derived from  "block_buffer::BlockBuffer" in Rust std modules.
    /// Ported by mizuky at 2020/09/01.
    /// 
    /// バイト列からなるデータをブロックごとに処理するためのバッファーです。
    /// RustCrypto/hasesのRustコードをC#コードへとポーティングする際に必要となった最小限の機能のみ有します。
    /// </summary>
    sealed class BlockBuffer
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

        /// <summary>
        /// 入力となるバイト列をバッファーに格納します。
        /// バッファーが満たされた時点で都度、指定されたアクションを実行してバッファをクリアします。
        /// 
        /// 入力となるバイト列サイズがバッファの現在の<see cref="Remaining"/>未満の場合、アクションは実行されません。
        /// 入力となるバイト列サイズがバッファの現在の<see cref="Remaining"/>以上の場合、そのサイズに応じて
        /// バッファの充足、アクションの実行、バッファのクリアという一連の動作を繰り返します。
        /// アクション<paramref name="f"/>に渡されるバイト列のサイズはバッファのサイズと一致します。
        /// </summary>
        /// <param name="input"></param>
        /// <param name="f"></param>
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

        public void PaddingLittleEndian(ulong dataLength, Action<byte[]> f)
        {
            DigestPadding(8, f);
            var b = dataLength.ToLittleEndianBytes();
            var n = _buffer.Length - b.Length;
            b.CopyTo(_buffer, n);
            f(_buffer);
            _pos = 0;
        }

        public void Reset()
        {
            _pos = 0;
        }

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
