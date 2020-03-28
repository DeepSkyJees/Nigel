

namespace Nigel.Security._461Demo
{
    static class Program
    {

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        static void Main()
        {
            //RsaKey rsa = RsaCrypt.GetRsaKeyExAsync().Result;
            RsaKey rsa = new RsaKey
            {
                PrivateKey = "<RSAKeyValue><Modulus>1+NjtMB8MGTX+SM4VsiBsbpZggUSNE2DsaBkSuRRckm0IJZDoNFImxjMICA1rc+iaE9Sq4JVWCG5Hny66ZPVvstbMJl2oLNNRNKt5PssRRly1zYperQ15pp1Z3CU+KfiHlY1kPPMm8sZCCIqtT3F8ApwLQB9NgU/F1jxW3Z0uAU=</Modulus><Exponent>AQAB</Exponent><P>8iPianV1hz2pm5iPcrtBvmqCp74vPDUsiR8ULYylekGyID5lpvsowyt022dRPls1cmTktfwOlrLpi8MNJMe4vw==</P><Q>5D7U47aow+aRlAmrktc0MCc1mkqxJ05vbemc57EDN+i67afFw2wWeWKEmkNGQzarwuTmu8U5nBPE850E35fcOw==</Q><DP>e/u90k8Ed9QuBVrPkKqrVinJbsSmAQklHa0JMu2CSaBb6cCpYh/WVmDo+/LiGbCwJDvfFAPVIHrJtOOR1lRhlw==</DP><DQ>JVzDrKLx7V+I5RrQIzFnMZq5g5BGf5CvXOao8KRhSn2mW9Di5qKC60vdOQNaNRZ192lQ+9vFGm+CBf7mFVPekQ==</DQ><InverseQ>gXt6b8HqKR75YLr5SRiN3Y5gKxTzJlHxA3W3ISzE3dg5e5pyEiEs+ZIcn1mr7CDRQE/a7yTTtlWILSzvr/5Cug==</InverseQ><D>yDl+ya8T/xYoMEp4ABTqJFm+lhX58kRJ9b3aBpOG7kZpJyf2BPrVKfNvrgPxhQhjifa3p5WzbY+pTxtDh0qzGxLgOC6aJiRQmz16neDJDBiqvbiRXE2nbg3vz5kbe29bNLLp4DPzrEoPskZOdEWOyorpk+V7anCfeC8AzmteKyk=</D></RSAKeyValue>",
                PublicKey = "<RSAKeyValue><Modulus>1+NjtMB8MGTX+SM4VsiBsbpZggUSNE2DsaBkSuRRckm0IJZDoNFImxjMICA1rc+iaE9Sq4JVWCG5Hny66ZPVvstbMJl2oLNNRNKt5PssRRly1zYperQ15pp1Z3CU+KfiHlY1kPPMm8sZCCIqtT3F8ApwLQB9NgU/F1jxW3Z0uAU=</Modulus><Exponent>AQAB</Exponent></RSAKeyValue>"
            };
            var eContent = "HuaGuiCQC".ToRsaEncryptEx(rsa);
            var dContent = eContent.RsaDecryptEx(rsa);
        }
    }
}
