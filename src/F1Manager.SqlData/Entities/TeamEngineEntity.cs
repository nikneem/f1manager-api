using System;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{

    [Table("TeamEngine")]
    public class TeamEngineEntity: Entity<Guid>
    {

        public Guid TeamId { get; set; }
        public Guid EngineId { get; set; }
        public int PointsGained { get; set; }
        [Column(TypeName = "decimal(5,2)")]
        public decimal WarnOffPercentage { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal BoughtFor { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? SoldFor { get; set; }
        public DateTimeOffset BoughtOn { get; set; }
        public DateTimeOffset? SoldOn { get; set; }

        [ForeignKey("TeamId")]
        public TeamEntity Team { get; set; }



    }
}
