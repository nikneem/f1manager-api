using System;

namespace F1Manager.Users.DataTransferObjects
{
    public class LoginAttemptDto
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Vector { get; set; }
    }
}
