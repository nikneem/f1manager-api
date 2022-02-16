using System;

namespace F1Manager.Leagues.DataTransferObjects;

public class LeagueMemberDto
{
    public Guid TeamId { get; set; }
    public bool IsMaintainer { get; set; }
}