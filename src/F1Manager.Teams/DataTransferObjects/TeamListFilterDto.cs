using System;
using System.Collections.Generic;
using F1Manager.Shared.Base;

namespace F1Manager.Teams.DataTransferObjects
{
    public class TeamsListFilterDto : PageFilterBase
    {

        public string Name { get; set; }
        public List<Guid> TeamIds { get; set; }
        public List<Guid> ExcludeTeamIds { get; set; }


    }
}
