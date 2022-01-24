using System;

namespace F1Manager.Admin.Results.DataTransferObjects
{
    public class RaceDriverResultDto
    {
        public Guid DriverId { get; set; }
        public int QualificationResult { get; set; }
        public int? SpringRaceResult { get; set; }
        public int RaceResult { get; set; }
        public bool IsFinished { get; set; }
        public Guid EngineId { get; set; }
        public Guid ChassisId { get; set; }
    }
}