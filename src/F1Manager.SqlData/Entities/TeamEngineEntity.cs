﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{

    [Table("TeamEngine")]
    public class TeamEngineEntity: Entity<Guid>
    {

        public Guid TeamId { get; set; }
        public Guid EngineId { get; set; }

        public string Name { get; set; }
        public string PictureUrl { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public int PointsGained { get; set; }
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
