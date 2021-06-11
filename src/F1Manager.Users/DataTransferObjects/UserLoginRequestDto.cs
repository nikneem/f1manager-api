using System;

namespace F1Manager.Users.DataTransferObjects
{
    public class UserLoginRequestDto
    {
        public Guid Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

    }
}
