using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Teams.Entities
{
    public class EngineEntity : TableEntity
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Value { get; set; }
        public string PictureUrl { get; set; }
        public decimal WeeklyWearDown { get; set; }
        public decimal MaximumWearDown { get; set; }
    }
}
