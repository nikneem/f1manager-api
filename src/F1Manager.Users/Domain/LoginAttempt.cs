using System;
using System.Security.Cryptography;
using System.Text;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;
using F1Manager.Users.DataTransferObjects;

namespace F1Manager.Users.Domain
{
    public class LoginAttempt : DomainModel<Guid>
    {
        public string RsaPrivateKey { get; }
        public string RsaPublicKey { get; }


        public UserLoginRequestDto DecryptUsernameAndPassword(UserLoginRequestDto dto)
        {
            using var importedRsa = new RSACng();
            var span = (ReadOnlySpan<byte>) Convert.FromBase64String(RsaPrivateKey);
            importedRsa.ImportPkcs8PrivateKey(span, out int _);
            var usernameBytes = Convert.FromBase64String(dto.Username);
            var passwordBytes = Convert.FromBase64String(dto.Password);

            var decryptedUsername = importedRsa.Decrypt(usernameBytes, RSAEncryptionPadding.OaepSHA256);
            var decryptedPassword = importedRsa.Decrypt(passwordBytes, RSAEncryptionPadding.OaepSHA256);

            return new UserLoginRequestDto
            {
                Id = dto.Id,
                Username = Encoding.UTF8.GetString(decryptedUsername),
                Password = Encoding.UTF8.GetString(decryptedPassword)
            };
        }


        public LoginAttempt(Guid id, string rsaPrivateKey) : base(id, TrackingState.Pristine)
        {
            RsaPrivateKey = rsaPrivateKey;
        }

        public LoginAttempt() : base(Guid.NewGuid(), TrackingState.New)
        {
            var rsa = RSA.Create();
            RsaPrivateKey = Convert.ToBase64String(rsa.ExportPkcs8PrivateKey());
            RsaPublicKey = Convert.ToBase64String(rsa.ExportSubjectPublicKeyInfo());
        }

    }
}