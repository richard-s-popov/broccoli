using System;
using System.Security.Cryptography;
using System.Text;

namespace BroccoliTrade.Logics.Infrastructure.Extensions
{
    /// <summary>
    /// Extensions for System.String
    /// </summary>
    public static class StringExtensions
    {
        private static readonly MD5CryptoServiceProvider _Md5Provider;
        static StringExtensions()
        {
            _Md5Provider = new MD5CryptoServiceProvider();
        }

        /// <summary>
        /// Handy String.Format call
        /// </summary>
        /// <param name="str"></param>
        /// <param name="args">Arguments</param>
        /// <returns></returns>
        public static string FormatString(this string str, params object[] args)
        {
            return String.Format(str, args);
        }

        /// <summary>
        /// Returns string's MD5 hash (PHP-compatible)
        /// </summary>
        /// <param name="input"></param>
        /// <returns>String's MD5 hash</returns>
        public static string Md5(this string input)
        {
            byte[] bs = Encoding.UTF8.GetBytes(input);
            bs = _Md5Provider.ComputeHash(bs);
            var s = new StringBuilder();
            foreach (byte b in bs)
            {
                s.Append(b.ToString("x2").ToLower());
            }
            string password = s.ToString();
            return password;
        }
    }
}
