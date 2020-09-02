using Mizuk.NCrypto.Hashes.Util;
using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mizuk.NCrypto.Hashes.Md4
{
    /// <summary>
    /// MD4ハッシュの機能を提供するクラスです。
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

        /// <summary>
        /// コンストラクタです。
        /// </summary>
        public Md4()
        {
            Reset();
        }

        void FinalizeInner()
        {
            var l = LengthBytes << 3;
            Buffer.Length64PaddingLittleEndian(l, x => State.ProcessBlock(x));
        }

        /// <summary>
        /// 入力データのバイト列を利用してMD4メッセージダイジェストの計算を続けます。
        /// </summary>
        /// <param name="input"></param>
        public void Update(byte[] input)
        {
            LengthBytes += (ulong)input.Length;
            Buffer.InputBlock(input, x => State.ProcessBlock(x));
        }

        /// <summary>
        /// MD4メッセージダイジェストの計算結果を引数で指定されたバッファに回収します。
        /// そしてオブジェクトの内部状態はダーティなままにします。
        /// </summary>
        /// <param name="output"></param>
        public void FinalizeIntoDirty(byte[] output)
        {
            FinalizeInner();

            foreach(var x in Enumerable.Range(0, output.Length).Where(x => x % 4 == 0)
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
            State = new Md4State();
            LengthBytes = 0;
            Buffer.Reset();
        }
    }
}
