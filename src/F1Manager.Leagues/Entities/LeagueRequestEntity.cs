using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Leagues.Entities
{
    public class LeagueRequestEntity : TableEntity
    {
        public DateTimeOffset? AcceptedOn { get; set; }
        public DateTimeOffset? DeclinedOn { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ExpiresOn { get; set; }
    }
}