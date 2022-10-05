using Nigel.Basic;

namespace Nigel.Security.Demo
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string? s = null;
            s.IsNotNullAll();
            var rsa = RsaCrypt.GetRsaKeyAsync().Result;
            var eContent = "Test".ToRsaEncrypt(rsa);
            var dContent = eContent.RsaDecrypt(rsa);
        }
    }
}