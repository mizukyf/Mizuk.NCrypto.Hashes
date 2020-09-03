namespace Mizuk.NCrypto.Hashes.Md4
{
    /// <summary>
    /// Trait for resetting hash instances
    /// </summary>
    public interface IReset
    {
        /// <summary>
        /// Reset hasher instance to its initial state and return current state.
        /// </summary>
        void Reset();
    }
}
