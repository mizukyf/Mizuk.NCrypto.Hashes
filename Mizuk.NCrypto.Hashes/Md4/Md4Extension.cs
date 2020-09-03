using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Mizuk.NCrypto.Hashes.Md4
{
    /// <summary>
    /// <see cref="Md4"/>とそれに関連するオブジェクトのために拡張メソッドを提供するユーティリティです。
    /// </summary>
    public static class Md4Extension
    {
        /// <summary>
        /// 指定された文字列をUTF16のバイト配列にした上でMD4ダイジェストメッセージに変換します。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static byte[] Digest(this Md4 self, IEnumerable<char> chars)
        {
            return Digest(self, Encoding.Unicode.GetBytes(chars.ToArray()));
        }
        /// <summary>
        /// 指定されたバイト配列をMD4ダイジェストメッセージに変換します。
        /// </summary>
        /// <param name="self"></param>
        /// <param name="bytes"></param>
        /// <returns></returns>
        public static byte[] Digest(this Md4 self, IEnumerable<byte> bytes)
        {
            self.Reset();
            self.Update(bytes.ToArray());
            return self.FinalizeFixedReset();
        }
        /// <summary>
        /// バイトのシーケンスを16進数の文字列表現に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hyphenSeparated">16進数のペアとペアの間にハイフンを挿入します</param>
        /// <returns></returns>
        public static string ToHexString(this IEnumerable<byte> value, bool hyphenSeparated = false)
        {
            if (hyphenSeparated) return BitConverter.ToString(value.ToArray());
            return BitConverter.ToString(value.ToArray()).Replace("-", string.Empty);
        }
    }
}
