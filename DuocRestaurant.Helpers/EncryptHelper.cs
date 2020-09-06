using System;
using System.Collections.Generic;
using System.Text;

namespace Helpers
{
    public class EncryptHelper
    {
        public static string SHA256(string randomString)
        {
            var crypt = new System.Security.Cryptography.SHA256Managed();
            var hash = new StringBuilder();
            byte[] crypto = crypt.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            foreach (byte theByte in crypto)
            {
                hash.Append(theByte.ToString("x2"));
            }
            crypt.Dispose();
            return hash.ToString();
        }
    }
}
