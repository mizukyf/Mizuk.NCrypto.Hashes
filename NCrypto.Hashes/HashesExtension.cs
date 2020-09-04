using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NCrypto.Hashes
{
    /// <summary>
    /// <see cref="Md4"/>や<see cref="Md5"/>とそれに関連するオブジェクトのために拡張メソッドを提供するユーティリティです。
    /// </summary>
    public static class HashesExtension
    {
        /// <summary>
        /// バイトのシーケンスを16進数の文字列表現に変換します。
        /// </summary>
        /// <param name="value"></param>
        /// <param name="hyphenSeparated">16進数のペアとペアの間にハイフンを挿入します</param>
        /// <param name="lowerCase">小文字を使用します</param>
        /// <returns></returns>
        public static string ToHexString(this IEnumerable<byte> value, bool hyphenSeparated = false, bool lowerCase = false)
        {
            return value.Select(x => string.Format(lowerCase ? "{0:x2}" : "{0:X2}", x))
                .Aggregate(new StringBuilder(),
                (a, b) => hyphenSeparated && a.Length > 0 ? a.Append('-').Append(b) : a.Append(b),
                x => x.ToString());
        }
    }
}
