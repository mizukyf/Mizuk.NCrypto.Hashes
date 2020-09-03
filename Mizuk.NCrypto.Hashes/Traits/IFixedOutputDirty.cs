namespace Mizuk.NCrypto.Hashes.Traits
{
    /// <summary>
    /// Trait for fixed-output digest implementations to use to retrieve the
    /// hash output.
    ///
    /// Usage of this trait in user code is discouraged. Instead use the
    /// [`FixedOutput::finalize_fixed`] or [`FixedOutput::finalize_fixed_reset`]
    /// methods.
    ///
    /// Types which impl this trait along with [`Reset`] will receive a blanket
    /// impl of [`FixedOutput`].
    /// </summary>
    public interface IFixedOutputDirty : IFixedOutput
    {
        /// <summary>
        /// Retrieve result into provided buffer and leave hasher in a dirty state.
        ///
        /// This method is expected to only be called once unless
        /// [`Reset::reset`] is called, after which point it can be
        /// called again and reset again (and so on). 
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
