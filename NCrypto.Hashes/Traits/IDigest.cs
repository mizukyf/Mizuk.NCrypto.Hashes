namespace NCrypto.Hashes.Traits
{
    /// <summary>
    /// The Digest trait specifies an interface common for digest functions.
    /// It's a convenience wrapper around Update, FixedOutput, Reset, Clone, and Default traits. 
    /// It also provides additional convenience methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IDigest<T> : IClone<T>, IFixedOutput, IReset, IUpdate<T> where T : IDigest<T>, IUpdate<T>
    {
        /// <summary>
        /// Retrieve result and consume hasher instance.
        /// </summary>
        /// <returns></returns>
        byte[] Finalize();

        /// <summary>
        /// Retrieve result and reset hasher instance.
        /// This method sometimes can be more efficient compared to hasher re-creation.
        /// </summary>
        /// <returns></returns>
        byte[] FinalizeReset();
    }

    static class DigestImpl
    {
        /// <summary>
        /// Retrieve result and consume hasher instance.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static byte[] Finalize<T>(this IDigest<T> self) where T : IDigest<T>
        {
            return self.FinalizeFixed();
        }

        /// <summary>
        /// Retrieve result and reset hasher instance.
        /// This method sometimes can be more efficient compared to hasher re-creation.
        /// </summary>
        /// <param name="self"></param>
        /// <returns></returns>
        public static byte[] FinalizeReset<T>(this IDigest<T> self) where T : IDigest<T>
        {
            var res = self.Clone().FinalizeFixed();
            self.Reset();
            return res;
        }
    }
}
