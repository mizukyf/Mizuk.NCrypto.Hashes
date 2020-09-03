namespace Mizuk.NCrypto.Hashes.Traits
{
    /// <summary>
    /// Trait for returning digest result with the fixed size
    /// </summary>
    public interface IFixedOutput
    {
        /// <summary>
        /// Output size for fixed output digest
        /// </summary>
        int OutputSize { get; }
        /// <summary>
        /// Write result into provided array and consume the hasher instance.
        /// </summary>
        /// <param name="output"></param>
        void FinalizeInto(byte[] output);
        /// <summary>
        /// Write result into provided array and reset the hasher instance.
        /// </summary>
        /// <param name="output"></param>
        void FinalizeIntoReset(byte[] output);
        /// <summary>
        /// Retrieve result and consume the hasher instance.
        /// </summary>
        /// <returns></returns>
        byte[] FinalizeFixed();
        /// <summary>
        /// Retrieve result and reset the hasher instance.
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
