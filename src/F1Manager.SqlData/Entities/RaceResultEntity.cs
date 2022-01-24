using System;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{

    [Table("RaceResults")]
    public class RaceResultEntity : Entity<Guid>
    {
        public Guid RaceWeekendId { get; set; }
        public Guid FastestLapDriverId { get; set; }
        public bool HasHalfPoints { get; set; }
        public DateTimeOffset CreatedOn { get; set; }
        public DateTimeOffset ModifiedOn { get; set; }
        
        [ForeignKey(nameof(RaceWeekendId))]
        public RaceWeekendEntity RaceWeekend { get; set; }

    }
}