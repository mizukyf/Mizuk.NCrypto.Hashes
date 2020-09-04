namespace NCrypto.Hashes.Traits
{
    /// <summary>
    /// このインターフェースはダイジェスト関数が<see cref="BlockSize"/>のサイズで
    /// データブロックを処理することを示すインターフェースです。
    /// </summary>
    public interface IBlockInput
    {
        /// <summary>
        /// ブロックサイズです。
        /// </summary>
        int BlockSize { get; }
    }
}
