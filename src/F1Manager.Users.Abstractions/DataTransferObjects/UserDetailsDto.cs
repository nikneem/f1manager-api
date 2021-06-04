using System;

namespace F1Manager.Users.DataTransferObjects
{
    public class UserDetailsDto
    {
        public Guid Id { get; set; }
        public string DisplayName { get;  set; }
        public string Username { get;  set; }
        public string Password { get;  set; }
        public string EmailAddress { get;  set; }
        public DateTimeOffset? DateEmailVerified { get;  set; }

        public DateTimeOffset RegisteredOn { get;  set; }
        public DateTimeOffset? LastLoginOn { get;  set; }
    }
}
