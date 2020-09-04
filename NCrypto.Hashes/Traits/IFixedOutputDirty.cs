namespace NCrypto.Hashes.Traits
{
    /// <summary>
    /// 固定長メッセージダイジェスト処理の結果を取得するためのインターフェースです。
    ///
    /// ユーザーコードでこのインターフェースが提供するメソッドを利用することは推奨されません。
    /// <see cref="IFixedOutput"/>が提供するメソッドを利用することをお奨めします。
    /// </summary>
    public interface IFixedOutputDirty : IFixedOutput
    {
        /// <summary>
        /// メッセージダイジェストの計算結果を引数で指定されたバッファに回収します。
        /// そしてオブジェクトの内部状態はダーティなままにします。
        /// </summary>
        /// <param name="output"></param>
        void FinalizeIntoDirty(byte[] output);
    }

    static class FixedOutputDirtyImpl
    {
        public static void FinalizeInto(this IFixedOutputDirty self, byte[] output)
        {
            self.FinalizeIntoDirty(output);
        }
        public static void FinalizeIntoReset<T>(this T self, byte[] output) where T: IFixedOutputDirty,IReset
        {
            self.FinalizeIntoDirty(output);
            self.Reset();
        }

    }
}
