using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Users.Entities
{
    public class RefreshTokenEntity : TableEntity
    {

        public string IpAddress { get; set; }
        public Guid UserId { get; set; }
        public DateTimeOffset GeneratedOn { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
        public bool IsActive { get; set; }
        public bool IsRevoked { get; set; }

    }
}
