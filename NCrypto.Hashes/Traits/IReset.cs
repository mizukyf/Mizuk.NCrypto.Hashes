namespace NCrypto.Hashes.Traits
{
    /// <summary>
    /// オブジェクトの内部状態をリセットするためのインターフェースです。
    /// </summary>
    public interface IReset
    {
        /// <summary>
        /// オブジェクトの内部状態を初期状態に戻します。
        /// </summary>
        void Reset();
    }
}
