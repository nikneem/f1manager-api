using System;
using System.Collections.Generic;

namespace F1Manager.Leagues.DataTransferObjects;

public class LeagueDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public List<LeagueMemberDto> Members { get; set; }
}