namespace Mizuk.NCrypto.Hashes.Traits
{
    /// <summary>
    /// Trait to indicate that digest function processes data in blocks of size BlockSize.
    /// The main usage of this trait is for implementing HMAC generically.
    /// </summary>
    public interface IBlockInput
    {
        /// <summary>
        /// Block size
        /// </summary>
        int BlockSize { get; }
    }
}
