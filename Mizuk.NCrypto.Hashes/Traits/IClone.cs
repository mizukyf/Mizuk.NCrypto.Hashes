namespace Mizuk.NCrypto.Hashes.Traits
{
    /// <summary>
    /// オブジェクトの明示的なコピーの機能を提供するインターフェースです。
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IClone<T>
    {
        /// <summary>
        /// オブジェクトをクローンします。
        /// </summary>
        /// <returns></returns>
        T Clone();
    }
}