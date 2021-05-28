using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Users.Entities
{
    public class UserEntity: TableEntity
    {
        public Guid SubjectId { get; set; }
        public string DisplayName { get;  set; }
        public string Password { get;  set; }

        public string EmailAddress { get;  set; }
        public string EmailVerificationCode { get;  set; }
        public DateTimeOffset? DateEmailVerified { get;  set; }
        public DateTimeOffset DueDateEmailVerified { get;  set; }

        public string LockoutReason { get;  set; }
        public bool IsAdministrator { get;  set; }

        public DateTimeOffset RegisteredOn { get;  set; }
        public DateTimeOffset? LastLoginOn { get;  set; }
    }
}
