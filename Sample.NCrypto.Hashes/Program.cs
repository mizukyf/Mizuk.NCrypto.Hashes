using NCrypto.Hashes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sample.NCrypto.Hashes
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("data  : {0}", args[0]);

            var data = Encoding.Unicode.GetBytes(args[0]);

            var md4 = new SharpCifs.Util.Md4();
            md4.Update(data);

            Console.WriteLine();
            Console.WriteLine("[MD4]");
            Console.WriteLine("ncrypto   : {0}", Md4.Digest(data).ToHexString(hyphenSeparated: true));
            Console.WriteLine("sharpcifs : {0}", md4.Digest().ToHexString(hyphenSeparated: true));

            Console.WriteLine();
            Console.WriteLine("[MD5]");
            
            var md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
            Console.WriteLine("ncrypto   : {0}", Md5.Digest(data).ToHexString(hyphenSeparated: true));
            Console.WriteLine("standard  : {0}", md5.ComputeHash(data).ToHexString(hyphenSeparated: true));

        }
    }
}
