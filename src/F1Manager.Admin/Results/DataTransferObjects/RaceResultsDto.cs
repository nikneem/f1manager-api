using System;
using System.Collections.Generic;

namespace F1Manager.Admin.Results.DataTransferObjects
{
    public class RaceResultsDto
    {
        public Guid RaceWeekendId { get; set; }
        public Guid FastestLapDriver { get; set; }
        public List<RaceDriverResultDto> DriverResults { get; set; }
        public bool HasHalfPoints { get; set; }
    }
}
