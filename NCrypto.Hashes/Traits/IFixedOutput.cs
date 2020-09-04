namespace NCrypto.Hashes.Traits
{
    /// <summary>
    /// 固定長のダイジェストメッセージを返すことを表すインターフェースです。
    /// </summary>
    public interface IFixedOutput
    {
        /// <summary>
        /// 出力のサイズです。
        /// </summary>
        int OutputSize { get; }
        /// <summary>
        /// 引数で指定された配列に結果を回収します。
        /// </summary>
        /// <param name="output"></param>
        void FinalizeInto(byte[] output);
        /// <summary>
        /// 引数で指定された配列に結果を回収し、オブジェクトの内部状態をリセットします。
        /// </summary>
        /// <param name="output"></param>
        void FinalizeIntoReset(byte[] output);
        /// <summary>
        /// 結果を取得します。
        /// </summary>
        /// <returns></returns>
        byte[] FinalizeFixed();
        /// <summary>
        /// 結果を取得し、オブジェクトの内部状態をリセットします。
        /// </summary>
        /// <returns></returns>
        byte[] FinalizeFixedReset();

    }

    static class FixedOutputImpl
    {
        public static byte[] FinalizeFixed(this IFixedOutput self)
        {
            var output = new byte[self.OutputSize];
            self.FinalizeInto(output);
            return output;
        }
        public static byte[] FinalizeFixedReset(this IFixedOutput self)
        {
            var output = new byte[self.OutputSize];
            self.FinalizeIntoReset(output);
            return output;
        }
    }
}
