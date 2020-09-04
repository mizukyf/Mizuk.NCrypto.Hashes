using NCrypto.Hashes.Traits;
using NCrypto.Hashes.Util;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NCrypto.Hashes
{
    /// <summary>
    /// MD4ハッシュの機能を提供するクラスです。
    /// </summary>
    /// <remarks>
    /// This code is derived from  RustCrypto/hashes.
    /// Ported by mizuky at 2020/09/01.
    /// </remarks>
    public sealed class Md4 : IFixedOutput, IFixedOutputDirty, IReset, IBlockInput, IClone<Md4>, IUpdate<Md4>, IDigest<Md4>
    {
        static internal readonly int _BlockSize = 64;
        static internal readonly int _OutputSize = 16;

        ulong LengthBytes;
        BlockBuffer Buffer = new BlockBuffer(_BlockSize);
        Md4State State;

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
        public Md4()
        {
            Reset();
        }

        void FinalizeInner()
        {
            var l = LengthBytes << 3;
            Buffer.Length64PaddingLittleEndian(l, State.ProcessBlock);
        }

        /// <summary>
        /// 入力データのバイト列を利用してMD4メッセージダイジェストの計算を続けます。
        /// </summary>
        /// <param name="input"></param>
        public void Update(byte[] input)
        {
            LengthBytes += (ulong)input.Length;
            Buffer.InputBlock(input, State.ProcessBlock);
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
            State = new Md4State(BlockSize);
            LengthBytes = 0;
            Buffer.Reset();
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
        /// このオブジェクトとその内部状態をクローンします。
        /// </summary>
        /// <returns></returns>
        public Md4 Clone()
        {
            var clone = new Md4();
            clone.LengthBytes = LengthBytes;
            clone.Buffer = Buffer.Clone();
            clone.State = State.Clone();
            return clone;
        }

        /// <summary>
        /// メソッドチェインの形式でデータを変換します。
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public Md4 Chain(byte[] data)
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
        /// 指定されたバイト配列をMD4メッセージダイジェストに変換します。
        /// </summary>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Digest(IEnumerable<byte> bytes)
        {
            var md4 = new Md4();
            md4.Update(bytes.ToArray());
            return md4.FinalizeFixed();
        }
        /// <summary>
        /// 指定された文字列をUTF16のバイト配列にした上でMD4メッセージダイジェストに変換します。
        /// </summary>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static byte[] Digest(IEnumerable<char> chars)
        {
            return Digest(Encoding.Unicode.GetBytes(chars.ToArray()));
        }
    }
}
