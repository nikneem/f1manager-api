using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Users.Entities
{
    public class LoginAttemptEntity : TableEntity
    {
        public string SecurityKey { get; set; }
        public string SecurityVector { get; set; }
        public DateTimeOffset IssuedOn { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
    }
}
