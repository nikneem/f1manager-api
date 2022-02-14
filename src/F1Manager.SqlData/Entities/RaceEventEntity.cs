using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{
    [Table("RaceEvents")]
    public class RaceEventEntity : Entity<Guid>
    {

        public int SeasonId { get; set; }
        public Guid CircuitId { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public DateTimeOffset Practice01 { get; set; }
        public DateTimeOffset Practice02 { get; set; }
        public DateTimeOffset? Practice03 { get; set; }
        public DateTimeOffset Qualification { get; set; }
        public DateTimeOffset? SprintRace { get; set; }
        public DateTimeOffset Race { get; set; }

        [ForeignKey("CircuitId")]
        public CircuitEntity Circuit { get; set; }

        public List<RaceResultEntity> Results { get; set; }
        public List<RaceDriverResultsEntity> DriverResults { get; set; }


    }
}
