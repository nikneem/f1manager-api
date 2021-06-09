using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{
    [Table("Teams")]
    public class TeamEntity: Entity<Guid>
    {


        public int SeasonId { get; set; }
        public Guid UserId { get; set; }

        [MinLength(2)]
        [MaxLength(100)]
        [Required]
        public string Name { get; set; }

        public int Points { get; set; }

        [Column(TypeName = "decimal(12, 2)")]
        public decimal Money { get; set; }

        public List<TeamEngineEntity> Engines { get; set; }

        public bool IsPublic { get; set; }

    }
}
