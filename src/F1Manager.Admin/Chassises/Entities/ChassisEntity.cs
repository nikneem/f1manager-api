using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Admin.Chassises.Entities
{
    public class ChassisEntity : TableEntity
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public double Value { get; set; }
        public double WeeklyWearOff { get; set; }
        public double MaxWearOff { get; set; }
        public string PictureUrl { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveUntil { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
    }
}