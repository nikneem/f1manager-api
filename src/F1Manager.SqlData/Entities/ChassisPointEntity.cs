using System;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{
    [Table("ChassisPoints")]
    public class ChassisPointEntity : Entity<Guid>
    {
        public int Position { get; set; }
        public int QualificationPoints { get; set; }
        public int RacePoints { get; set; }
        public int SprintRacePoints { get; set; }

        public ChassisPointEntity(int position, int quali, int race, int sprint)
        {
            Id = Guid.NewGuid();
            Position = position;
            QualificationPoints = quali;
            RacePoints = race;
            SprintRacePoints = sprint;
        }
        public ChassisPointEntity()
        {

        }
    }
}