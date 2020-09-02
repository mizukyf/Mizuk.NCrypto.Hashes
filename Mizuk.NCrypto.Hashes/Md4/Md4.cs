using Mizuk.NCrypto.Hashes.Util;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuk.NCrypto.Hashes.Md4
{
    /// <summary>
    /// </summary>
    /// <remarks>
    /// This code is derived from  RustCrypto/hashes.
    /// Ported by mizuky at 2020/09/01.
    /// </remarks>
    public sealed class Md4
    {
        static internal readonly int BlockSize = 64;

        ulong LengthBytes;
        readonly BlockBuffer Buffer = new BlockBuffer(BlockSize);
        Md4State State;

        public Md4()
        {
            Reset();
        }

        void FinalizeInner()
        {
            var l = LengthBytes << 3;
            Buffer.Length64PaddingLittleEndian(l, x => State.ProcessBlock(x));
        }

        public void Update(byte[] input)
        {
            LengthBytes += (ulong)input.Length;
            Buffer.InputBlock(input, x => State.ProcessBlock(x));
        }

        public byte[] Digest()
        {
            FinalizeInner();
            return State.SelectMany(x => x.ToLittleEndianBytes()).ToArray();
        }

        public void Reset()
        {
            State = new Md4State();
            LengthBytes = 0;
            Buffer.Reset();
        }
    }
}
