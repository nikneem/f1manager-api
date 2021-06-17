using System;

namespace F1Manager.Users.DataTransferObjects
{
    public class LoginAttemptDto
    {
        public Guid Id { get; set; }
        public string RsaPublicKey { get; set; }
    }
}
