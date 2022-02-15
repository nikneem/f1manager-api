using System;
using System.Security.Cryptography;
using System.Text;
using F1Manager.Shared.Constants;

namespace F1Manager.Shared.Helpers
{
    public static class Randomize
    {

        private static readonly RandomNumberGenerator Random;
        private static readonly string Capitals = "ABCDEFGHIJKLMNOPQRSTUVWYXZ";
        private static readonly string AlphaPool = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string NumericPool = "0123456789";
        private static readonly string CaseSensitiveAlphaNumericPool = AlphaPool + NumericPool + Capitals;

        public static string GenerateEmailVerificationCode()
        {
            var secret = new StringBuilder();
            do
            {
                secret.Append(Capitals.Substring(Random.Next(0,Capitals.Length), 1));
            } while (secret.Length < Defaults.EmailVerificationLength);

            return secret.ToString();
        }

        public static string GenerateRefreshToken()
        {
            var refreshToken = new StringBuilder();
            do
            {
                refreshToken.Append(CaseSensitiveAlphaNumericPool.Substring(Random.Next(0,CaseSensitiveAlphaNumericPool.Length), 1));
            } while (refreshToken.Length < Defaults.RefreshTokenLength);

            return refreshToken.ToString();
        }

        private static int Next(this RandomNumberGenerator generator, int min, int max)
        {
            int minimum = min;
            int maximum = max-1;

            var bytes = new byte[sizeof(int)]; 
            generator.GetNonZeroBytes(bytes);
            var val = BitConverter.ToInt32(bytes);
            var result = ((val - minimum) % (maximum - minimum + 1) + (maximum - minimum) + 1) % (maximum - minimum + 1) + minimum;
            return result;
        }

        static Randomize()
        {
            Random = RandomNumberGenerator.Create();
        }
    }
}
