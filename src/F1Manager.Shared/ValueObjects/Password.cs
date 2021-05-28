using F1Manager.Shared.Helpers;

namespace F1Manager.Shared.ValueObjects
{
    public sealed class Password
    {
        public string EncryptedPassword { get; private set; }

        public bool Validate(string plainTextPassword)
        {
            return SecurePasswordHasher.Verify(plainTextPassword, EncryptedPassword);
        }

        public void SetPassword(string plainTextPassword)
        {
            EncryptedPassword = SecurePasswordHasher.Hash(plainTextPassword);
        }


        public Password(string hashedPassword)
        {
            EncryptedPassword = hashedPassword;
        }

        public static Password Create(string plainTextPassword)
        {
            var hashedPassword = SecurePasswordHasher.Hash(plainTextPassword);
            return new Password(hashedPassword);
        }

    }
}
