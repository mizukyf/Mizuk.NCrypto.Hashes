using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mizuk.NCrypto.Hashes.Util
{
    /// <summary>
    /// This code is derived from  "std::slice::ChunksExact" in Rust std modules.
    /// Ported by mizuky at 2020/09/01.
    /// 
    /// 予め指定されたチャンクサイズでバイト配列を繰り返し処理するためのクラスです。
    /// <see cref="GetEnumerator"/>が返す列挙子はあくまでも予め定められたチャンクサイズでバイト配列を切り出します。
    /// 元のデータにチャンクサイズ未満の部分が残る場合、<see cref="GetRemainder"/>はそのバイト配列を返します。
    /// </summary>
    sealed class ChunksExact : IEnumerable<byte[]>
    {
        readonly byte[] _values;
        readonly int _chunkSize;
        byte[] _remainder;
        bool _remainderReady;

        internal ChunksExact(byte[] values, int chunkSize)
        {
            _values = values;
            _chunkSize = chunkSize;
            _remainder = new byte[0];
        }
        /// <summary>
        /// 列挙子を取得します。
        /// <see cref="GetEnumerator"/>が返す列挙子はあくまでも予め定められたチャンクサイズでバイト配列を切り出します。
        /// 列挙の後、元のデータにチャンクサイズ未満の部分が残る場合、<see cref="GetRemainder"/>を呼び出します。
        /// </summary>
        /// <returns></returns>
        public IEnumerator<byte[]> GetEnumerator()
        {
            byte[] chunk = null;
            foreach (var i in Enumerable.Range(0, _values.Length))
            {
                var ii = i % _chunkSize;
                if (ii == 0)
                {
                    chunk = new byte[_chunkSize];
                }

                chunk[ii] = _values[i];

                if (ii == (_chunkSize - 1))
                {
                    yield return chunk;
                }
                else if(i == (_values.Length - 1))
                {
                    _remainder = chunk.Take(ii + 1).ToArray();
                    _remainderReady = true;
                }
            }
        }
        /// <summary>
        /// 列挙子を取得します。
        /// <see cref="GetEnumerator"/>が返す列挙子はあくまでも予め定められたチャンクサイズでバイト配列を切り出します。
        /// 列挙の後、元のデータにチャンクサイズ未満の部分が残る場合、<see cref="GetRemainder"/>を呼び出します。
        /// </summary>
        /// <returns></returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// <see cref="GetEnumerator"/>が返す列挙子による列挙の後、元のデータにチャンクサイズ未満の部分が残る場合、
        /// このメソッドは長さ1以上、チャンクサイズ未満のバイト配列を返します。
        /// </summary>
        /// <returns></returns>
        public byte[] GetRemainder()
        {
            if (!_remainderReady)
            {
                throw new InvalidOperationException("remainder is not be ready. " +
                    "please call GetEnumerator() before this method.");
            }
            return _remainder;
        }
    }
}
