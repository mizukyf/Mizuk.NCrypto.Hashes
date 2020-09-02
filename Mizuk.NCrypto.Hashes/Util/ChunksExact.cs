using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mizuk.NCrypto.Hashes.Util
{
    /// <summary>
    /// This code is derived from  "std::slice::ChunksExact" in Rust std modules.
    /// Ported by mizuky at 2020/09/01.
    /// </summary>
    sealed class ChunksExact : IEnumerable<byte[]>
    {
        readonly byte[] _values;
        readonly int _chunkSize;
        byte[] _remainder;

        internal ChunksExact(byte[] values, int chunkSize)
        {
            _values = values;
            _chunkSize = chunkSize;
            _remainder = new byte[0];
        }

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
                }
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public byte[] GetRemainder()
        {
            return _remainder;
        }
    }
}
