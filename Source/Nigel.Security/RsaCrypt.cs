using NETCore.Encrypt;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Basic.Security
{
    /// <summary>
    /// 
    /// </summary>
    public static class RsaCrypt
    {
        /// <summary>
        /// newCore
        /// </summary>
        /// <returns></returns>
        public static async Task<RsaKey> GetRsaKeyAsync()
        {
            var newRsaKey = EncryptProvider.CreateRsaKey();

            var rsaKey = new RsaKey
            {
                PrivateKey = newRsaKey.PrivateKey,
                PublicKey = newRsaKey.PublicKey
            };

            return await Task.FromResult(rsaKey);


        }

        /// <summary>
        /// NetCore RSA 加密
        /// </summary>
        /// <param name="contentString">The content string.</param>
        /// <param name="nRSAKey">The n RSA key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">nRSAKey</exception>
        public static  string ToRsaEncrypt(this string contentString, RsaKey rsaKey)
        {
            if (rsaKey == null)
            {
                throw new ArgumentNullException(nameof(rsaKey));
            }
            var encryptString = EncryptProvider.RSAEncrypt(rsaKey.PublicKey, contentString);
            return encryptString.ToLower() ;
        }

        /// <summary>
        /// NetCore RSA 解密
        /// </summary>
        /// <param name="contentString">The content string.</param>
        /// <param name="nRSAKey">The n RSA key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">nRSAKey</exception>
        public static string RsaDecrypt(this string contentString, RsaKey nRSAKey)
        {
            try
            {
                if (nRSAKey == null)
                {
                    throw new ArgumentNullException(nameof(nRSAKey));
                }
                var encryptString = EncryptProvider.RSADecrypt(nRSAKey.PrivateKey, contentString.ToUpper());
                return encryptString;
            }
            catch (Exception ex)
            {

                throw ex;
            }
           
        }


        /// <summary>
        /// NetFramework RSA 加密
        /// </summary>
        /// <returns></returns>
        public static async Task<RsaKey> GetRsaKeyExAsync()
        {
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                string privateKey = rsa.ToXmlString(true);
                string publicKey = rsa.ToXmlString(false);
                var rsaKey = new RsaKey
                {
                    PrivateKey = privateKey,
                    PublicKey = publicKey
                };

                return await Task.FromResult(rsaKey);
            }
            catch (Exception ex)
            {

                throw ex;
            }
           


        }

        /// <summary>
        /// NetFramework RSA 加密
        /// </summary>
        /// <param name="contentString">The content string.</param>
        /// <param name="nRsaKey">The n RSA key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">nRSAKey</exception>
        public static string ToRsaEncryptEx(this string contentString, RsaKey nRsaKey)
        {
            try
            {
                if (nRsaKey == null)
                {
                    throw new ArgumentNullException(nameof(nRsaKey));
                }
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(nRsaKey.PublicKey);
                byte[] cipherbytes = rsa.Encrypt(Encoding.UTF8.GetBytes(contentString), false);

                return Convert.ToBase64String(cipherbytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }


        }

        /// <summary>
        /// NetFramework RSA 解密
        /// </summary>
        /// <param name="contentString">The content string.</param>
        /// <param name="nRSAKey">The n RSA key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException">nRSAKey</exception>
        public static string RsaDecryptEx(this string contentString, RsaKey nRSAKey)
        {
            try
            {
                if (nRSAKey == null)
                {
                    throw new ArgumentNullException(nameof(nRSAKey));
                }
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
                rsa.FromXmlString(nRSAKey.PrivateKey);
                var cipherbytes = rsa.Decrypt(Convert.FromBase64String(contentString), false);
                return Encoding.UTF8.GetString(cipherbytes);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
    }
}
