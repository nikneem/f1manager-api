using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace F1Manager.Shared.Helpers
{
    public static class SecurePasswordHasher
    {

        private const int SaltSize = 16;
        
        public static string Hash(string password, string salt, int iterations = 10000)
        {
            var s = Convert.FromBase64String(salt);
            if (s.Length != SaltSize)
            {
                throw new Exception("Invalid salt size");
            }

            return Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: s,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));
        }

        public static string GenerateSalt()
        {
            byte[] salt = new byte[SaltSize];
            RandomNumberGenerator.Fill(salt);
            return Convert.ToBase64String(salt);
        }
        

        public static bool Verify(string password, string hashedPassword, string salt)
        {
            var hash = Hash(password, salt);
            return hashedPassword == hash;
        }
    }
}
