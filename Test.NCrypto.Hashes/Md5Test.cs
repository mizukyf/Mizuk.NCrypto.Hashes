using NCrypto.Hashes;
using NUnit.Framework;
using System;
using System.Linq;
using System.Text;

namespace Test.NCrypto.Hashes
{
    [TestFixture]
    public class Md5Test
    {
        [TestCase("a")]
        [TestCase("ab")]
        [TestCase("abc")]
        [TestCase("0")]
        [TestCase("01")]
        [TestCase("012")]
        [TestCase("@")]
        [TestCase("@*")]
        [TestCase("@*!")]
        [TestCase("あ")]
        [TestCase("あい")]
        [TestCase("あいう")]
        public void CompareWithStandardImpl(string src)
        {
            var data = Encoding.Unicode.GetBytes(src);

            var standardMd5 = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(data);
            var ncryptMd5 = Md5.Digest(data);

            Assert.That(standardMd5, Is.EqualTo(ncryptMd5));
            Assert.That(BitConverter.ToString(standardMd5), Is.EqualTo(ncryptMd5.ToHexString(hyphenSeparated: true)));
            Assert.That(BitConverter.ToString(standardMd5).Replace("-", string.Empty), Is.EqualTo(ncryptMd5.ToHexString(hyphenSeparated: false)));
        }
        [TestCase('x', 0)]
        [TestCase('x', 10)]
        [TestCase('x', 100)]
        [TestCase('x', 1000)]
        [TestCase('ｘ', 10)]
        [TestCase('ｘ', 100)]
        [TestCase('ｘ', 1000)]
        [TestCase('ん', 10)]
        [TestCase('ん', 100)]
        [TestCase('ん', 1000)]
        public void CompareWithStandardImpl(char ch, int len)
        {
            var data = Encoding.Unicode.GetBytes(Enumerable.Repeat(ch, len).ToArray());

            var standardMd5 = new System.Security.Cryptography.MD5CryptoServiceProvider().ComputeHash(data);
            var ncryptMd5 = Md5.Digest(data);

            Assert.That(standardMd5, Is.EqualTo(ncryptMd5));
            Assert.That(BitConverter.ToString(standardMd5), Is.EqualTo(ncryptMd5.ToHexString(hyphenSeparated: true)));
            Assert.That(BitConverter.ToString(standardMd5).Replace("-", string.Empty), Is.EqualTo(ncryptMd5.ToHexString(hyphenSeparated: false)));
        }
    }
}
