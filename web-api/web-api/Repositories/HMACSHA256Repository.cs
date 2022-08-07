using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System;
using System.Text;
using web_api.Interfaces;

namespace web_api.Repositories
{
    public class HMACSHA256Repository : IPasswordHasher
    {
        public string Hash(string password)
        {
            const string SALT = "CGYzqeN4plZekNC88Umm1Q==";

            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
               password: password,
               salt: Encoding.UTF8.GetBytes(SALT),
               prf: KeyDerivationPrf.HMACSHA256,
               iterationCount: 100000,
               numBytesRequested: 256 / 8));

            return hashedPassword;
        }
    }
}
