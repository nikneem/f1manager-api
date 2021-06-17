using System;
using System.Text;
using F1Manager.Shared.Constants;

namespace F1Manager.Shared.Helpers
{
    public static class Randomize
    {

        private static readonly Random Random;
        private static readonly string Capitals = "ABCDEFGHIJKLMNOPQRSTUVWYXZ";
        private static readonly string AlphaPool = "abcdefghijklmnopqrstuvwxyz";
        private static readonly string NumericPool = "0123456789";
        private static readonly string AlphaNumericPool = AlphaPool + NumericPool;
        private static readonly string CaseSensitiveAlphaNumericPool = AlphaPool + NumericPool + Capitals;

        public static string GenerateEmailVerificationCode()
        {
            var secret = new StringBuilder();
            do
            {
                secret.Append(Capitals.Substring(Random.Next(Capitals.Length), 1));
            } while (secret.Length < Defaults.EmailVerificationLength);

            return secret.ToString();
        }

        public static string GenerateRefreshToken()
        {
            var refreshToken = new StringBuilder();
            do
            {
                refreshToken.Append(CaseSensitiveAlphaNumericPool.Substring(Random.Next(CaseSensitiveAlphaNumericPool.Length), 1));
            } while (refreshToken.Length < Defaults.RefreshTokenLength);

            return refreshToken.ToString();
        }


        static Randomize()
        {
            Random = new Random(DateTime.UtcNow.Minute * 
                                 DateTime.UtcNow.Second * 
                                 DateTime.UtcNow.Millisecond);
        }
    }
}
