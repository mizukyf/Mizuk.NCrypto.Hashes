namespace Mizuk.NCrypto.Hashes.Traits
{
    /// <summary>
    /// Trait for updating digest state with input data.
    /// </summary>
    public interface IUpdate<T> where T : IUpdate<T>
    {
        /// <summary>
        /// Digest input data.
        /// This method can be called repeatedly, e.g. for processing streaming messages.
        /// </summary>
        /// <param name="data"></param>
        void Update(byte[] data);

        /// <summary>
        /// Digest input data in a chained manner.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        T Chain(byte[] data);
    }

    static class UpdateImpl
    {
        public static T Chain<T>(this T self, byte[] data) where T : IUpdate<T>
        {
            self.Update(data);
            return self;
        }
    }
}
