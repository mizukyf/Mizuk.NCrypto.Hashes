namespace NCrypto.Hashes.Traits
{
    /// <summary>
    /// ダイジェスト関数の内部状態を入力データで更新するためのインターフェースです。
    /// </summary>
    public interface IUpdate<T> where T : IUpdate<T>
    {
        /// <summary>
        /// 入力データを処理します。
        /// このメソッドは繰り返し呼ぶことができます。
        /// </summary>
        /// <param name="data"></param>
        void Update(byte[] data);

        /// <summary>
        /// メソッドチェインの形式でデータを変換します。
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
