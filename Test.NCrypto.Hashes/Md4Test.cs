using NCrypto.Hashes;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.NCrypto.Hashes
{

    [TestFixture]
    public class Md4Test
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
        public void CompareWithSharpCifsImpl(string src)
        {
            var data = Encoding.Unicode.GetBytes(src);

            var md4 = new SharpCifs.Util.Md4();
            md4.Update(data);

            Assert.That(Md4.Digest(data), Is.EqualTo(md4.Digest()));
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
        public void CompareWithSharpCifsImpl(char ch, int len)
        {
            var data = Encoding.Unicode.GetBytes(Enumerable.Repeat(ch, len).ToArray());

            var md4 = new SharpCifs.Util.Md4();
            md4.Update(data);

            Assert.That(Md4.Digest(data), Is.EqualTo(md4.Digest()));
        }
    }
}
