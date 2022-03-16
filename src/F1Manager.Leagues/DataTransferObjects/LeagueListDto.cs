using System;

namespace F1Manager.Leagues.DataTransferObjects
{
    public  class LeagueListDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Members { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
    }
}
