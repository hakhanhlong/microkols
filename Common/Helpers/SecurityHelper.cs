using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Common.Helpers
{
   public  class SecurityHelper
    {
        public static string MD5Hash(string str)
        {
            byte[] buffer = MD5.Create().ComputeHash(Encoding.UTF8.GetBytes(str.ToLower()));
            var builder = new StringBuilder();
            foreach (byte t in buffer)
            {
                builder.AppendFormat("{0:x2}", t);
            }
            return builder.ToString();
        }

        public static string SHA256Hash(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                var hash = BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
                return hash;
            }
        }

        public static string HashPassword(string salt, string password)
        {
            return SHA256Hash($"{password}{salt}");
        }
        public static string GenerateSalt()
        {
            byte[] bytes = new byte[128 / 8];
            using (var keyGenerator = RandomNumberGenerator.Create())
            {
                keyGenerator.GetBytes(bytes);

                return BitConverter.ToString(bytes).Replace("-", "").ToLower();
            }
        }

        public static string Base64Encode(string plainText)
        {
            var plainTextBytes = System.Text.Encoding.UTF8.GetBytes(plainText);
            return System.Convert.ToBase64String(plainTextBytes);
        }
        public static string Base64Decode(string base64EncodedData)
        {
            var base64EncodedBytes = System.Convert.FromBase64String(base64EncodedData);
            return System.Text.Encoding.UTF8.GetString(base64EncodedBytes);
        }
    }
}
