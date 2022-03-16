using System;
using System.Collections.Generic;
using F1Manager.Shared.Base;

namespace F1Manager.Teams.DataTransferObjects;

public class TeamSearchDto: PageFilterBase
{
    public List<Guid> TeamIds { get; set; }
}