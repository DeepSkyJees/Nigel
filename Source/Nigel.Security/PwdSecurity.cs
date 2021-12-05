using Nigel.Basic.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Nigel.Security
{
    public static class PwdSecurity
    {
        /// <summary>
        /// Creates the password hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="passwordHash">The password hash.</param>
        /// <param name="passwordSalt">The password salt.</param>
        /// <exception cref="AppException">
        /// 密码不能为空
        /// or
        /// 密码不能为空字符串
        /// </exception>
        public static void CreatePasswordHash(this string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using var hmac = new HMACSHA512();
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        }


        /// <summary>
        /// Verifies the password hash.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="storedHash">The stored hash.</param>
        /// <param name="storedSalt">The stored salt.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="AppException">
        /// 密码不能为空
        /// or
        /// 密码不能为空字符串
        /// or
        /// 密码哈希错误
        /// or
        /// 密码盐值错误
        /// </exception>
        public static bool VerifyPasswordHash(this string password, byte[] storedHash, byte[] storedSalt)
        {
            if (password == null) throw new AppException("密码不能为空");
            if (string.IsNullOrWhiteSpace(password)) throw new AppException("密码不能为空字符串");
            if (storedHash.Length != 64) throw new AppException("密码哈希错误");
            if (storedSalt.Length != 128) throw new AppException("密码盐值错误");

            using (var hmac = new HMACSHA512(storedSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                for (var i = 0; i < computedHash.Length; i++)
                    if (computedHash[i] != storedHash[i])
                        return false;
            }

            return true;
        }
    }
}