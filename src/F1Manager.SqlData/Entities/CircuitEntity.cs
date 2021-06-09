using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{
    [Table("Circuits")]
    public class CircuitEntity: Entity<Guid>
    {


        public string Name { get; set; }
        public int FirstGrandPrix { get; set; }
        public int NumberOfLaps { get; set; }
        [Column(TypeName = "decimal(5, 3)")]
        public decimal Length { get; set; }
        [Column(TypeName = "decimal(7, 3)")]
        public decimal RaceDistance { get; set; }
        public string LapRecord { get; set; }


    }
}
