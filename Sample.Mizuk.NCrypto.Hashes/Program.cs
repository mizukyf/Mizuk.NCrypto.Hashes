using Mizuk.NCrypto.Hashes.Md4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.Mizuk.NCrypto.Hashes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("original  : {0}", args[0]);
            Console.WriteLine("ncrypto   : {0}", new Md4().Digest(args[0]).ToHexString(hyphenSeparated: true));

            var md4 = new SharpCifs.Util.Md4();
            md4.Update(Encoding.Unicode.GetBytes(args[0]));

            Console.WriteLine("sharpcifs : {0}", md4.Digest().ToHexString(hyphenSeparated: true));
        }
    }
}
