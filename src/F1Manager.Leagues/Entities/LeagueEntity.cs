using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Leagues.Entities
{
    public class LeagueEntity : TableEntity
    {
        public int SeasonId { get; set; }
        public Guid OwnerId { get; set; }
        public string Name { get; set; }
        public int MembersCount { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}