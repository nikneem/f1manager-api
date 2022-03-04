using System;

namespace F1Manager.Leagues.DataTransferObjects;

public class LeagueRequestDto
{
    public Guid TeamId { get; set; }
    public string TeamName { get; set; }
    public DateTimeOffset CreatedOn { get; set; }
    public DateTimeOffset ExpiresOn { get; set; }
}