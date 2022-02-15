using System;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{

    [Table("RaceDriverResults")]
    public class RaceDriverResultsEntity : Entity<Guid>
    {
        public Guid RaceWeekendId { get; set; }
        public Guid DriverId { get; set; }
        public Guid EngineId { get; set; }
        public Guid ChassisId { get; set; }
        public int QualificationResult { get; set; }
        public int? SprintRaceResult { get; set; }
        public int RaceResult { get; set; }
        public bool IsFinished { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }

        [ForeignKey(nameof(RaceWeekendId))]
        public RaceEventEntity RaceEvent { get; set; }
    }
}