using F1Manager.Shared.Helpers;

namespace F1Manager.Shared.ValueObjects
{
    public sealed class Password
    {
        public string EncryptedPassword { get; private set; }
        public string Salt { get; private set; }

        public bool Validate(string plainTextPassword)
        {
            return SecurePasswordHasher.Verify(plainTextPassword, EncryptedPassword, Salt);
        }

        public void SetPassword(string plainTextPassword)
        {
            Salt = SecurePasswordHasher.GenerateSalt();
            EncryptedPassword = SecurePasswordHasher.Hash(plainTextPassword, Salt);
        }


        public Password(string hashedPassword, string salt)
        {
            EncryptedPassword = hashedPassword;
            Salt = salt;
        }

        public static Password Create(string plainTextPassword)
        {
            var salt = SecurePasswordHasher.GenerateSalt();
            var hashedPassword = SecurePasswordHasher.Hash(plainTextPassword, salt);
            return new Password(hashedPassword, salt);
        }

    }
}
