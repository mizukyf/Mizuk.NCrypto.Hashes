using Mizuk.NCrypto.Hashes.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Mizuk.NCrypto.Hashes.Md4
{
    /// <summary>
    /// This code is derived from  RustCrypto/hashes.
    /// Ported by mizuky at 2020/09/01.
    /// </summary>
    sealed class Md4State : IEnumerable<uint>
    {
        readonly uint[] _values = { 0x6745_2301, 0xEFCD_AB89, 0x98BA_DCFE, 0x1032_5476 };

        uint this[int i]
        {
            get
            {
                return _values[i];
            }
            set
            {
                _values[i] = value;
            }
        }

        public IEnumerator<uint> GetEnumerator()
        {
            foreach(var i in _values)
            {
                yield return i;
            }
        }

        public void ProcessBlock(byte[] input)
        {
            if (input.Length != Md4.BlockSize)
            {
                throw new ArgumentException(string.Format("block' size must be ", Md4.BlockSize));
            }

            var a = _values[0];
            var b = _values[1];
            var c = _values[2];
            var d = _values[3];

            // load block to data
            var data = new uint[16];
            foreach (var x in input.ChunksExact(4).Select((e, i) => new { DataIndex = i, InputChunk = e }))
            {
                data[x.DataIndex] = x.InputChunk.FromLittleEndianBytes();
            }

            // round 1
            foreach (var i in new int[] { 0, 4, 8, 12 })
            {
                a = op1(a, b, c, d, data[i], 3);
                d = op1(d, a, b, c, data[i + 1], 7);
                c = op1(c, d, a, b, data[i + 2], 11);
                b = op1(b, c, d, a, data[i + 3], 19);
            }

            // round 2
            foreach (var i in new int[] { 0, 1, 2, 3 })
            {
                a = op2(a, b, c, d, data[i], 3);
                d = op2(d, a, b, c, data[i + 4], 5);
                c = op2(c, d, a, b, data[i + 8], 9);
                b = op2(b, c, d, a, data[i + 12], 13);
            }

            // round 3
            foreach (var i in new int[] { 0, 2, 1, 3 })
            {
                a = op3(a, b, c, d, data[i], 3);
                d = op3(d, a, b, c, data[i + 8], 9);
                c = op3(c, d, a, b, data[i + 4], 11);
                b = op3(b, c, d, a, data[i + 12], 15);
            }

            _values[0] += a;
            _values[1] += b;
            _values[2] += c;
            _values[3] += d;
        }

        uint f(uint x, uint y, uint z)
        {
            return (x & y) | (~x & z);
        }

        uint g(uint x, uint y, uint z)
        {
            return (x & y) | (x & z) | (y & z);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        uint h(uint x, uint y, uint z)
        {
            return x ^ y ^ z;
        }

        uint op1(uint a, uint b, uint c, uint d, uint k,int s)
        {
            var t = a + f(b, c, d) + k;
            return t.RotateLeft(s);
        }

        uint op2(uint a, uint b, uint c, uint d, uint k, int s)
        {
            var t = a + g(b, c, d) + k + 0x5A82_7999;
            return t.RotateLeft(s);
        }

        uint op3(uint a, uint b, uint c, uint d, uint k, int s)
        {
            var t = a + h(b, c, d) + k + 0x6ED9_EBA1;
            return t.RotateLeft(s);
        }
    }
}
