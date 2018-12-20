using System;
using Nigel.Basic.Security;

namespace Nigel.Security.Demo
{
    class Program
    {
        static void Main(string[] args)
        {
            var rsa =    RsaCrypt.GetRsaKeyAsync().Result;
            string eContent = "Test".ToRsaEncrypt(rsa);
            string dContent = eContent.RsaDecrypt(rsa);
        }

    }
}
