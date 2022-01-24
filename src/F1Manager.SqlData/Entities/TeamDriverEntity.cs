using System;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{
    public sealed class TeamDriverEntity: Entity<Guid>
    {
        public Guid TeamId { get; set; }
        public Guid DriverId { get; set; }
        public int PointsGained { get; set; }

        [Column(TypeName = "decimal(10, 2)")]
        public decimal PurchasePrice { get; set; }
        [Column(TypeName = "decimal(10, 2)")]
        public decimal? SellingPrice { get; set; }

        public bool IsFirstDriver { get; set; }
        public DateTimeOffset BoughtOn { get; set; }
        public DateTimeOffset? SoldOn { get; set; }

        [ForeignKey("TeamId")]
        public TeamEntity Team { get; set; }


    }
}
