namespace NCrypto.Hashes.Traits
{
    /// <summary>
    /// ダイジェスト関数のための共通インターフェースです。
    /// <see cref="IUpdate{T}"/>、<see cref="IFixedOutput"/>、<see cref="IReset"/>、
    /// そして<see cref="IClone{T}"/>を包含する便利なインターフェースです。
    /// 加えていくつかの追加のメソッドも提供します。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDigest<T> : IClone<T>, IFixedOutput, IReset, IUpdate<T> where T : IDigest<T>, IUpdate<T>
    {
        /// <summary>
        /// 結果を取得します。
        /// </summary>
        /// <returns></returns>
        byte[] Finalize();

        /// <summary>
        /// 結果を取得しオブジェクトの内部状態をリセットします。
        /// このメソッドはオブジェクトの再作成を行うのに比べて効率的になることがあります。
        /// </summary>
        /// <returns></returns>
        byte[] FinalizeReset();
    }

    static class DigestImpl
    {
        public static byte[] Finalize<T>(this IDigest<T> self) where T : IDigest<T>
        {
            return self.FinalizeFixed();
        }

        public static byte[] FinalizeReset<T>(this IDigest<T> self) where T : IDigest<T>
        {
            var res = self.Clone().FinalizeFixed();
            self.Reset();
            return res;
        }
    }
}
