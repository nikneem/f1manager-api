using System;

namespace F1Manager.Admin.Engines.DataTransferObjects
{
    public class EngineDetailsDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public string PictureUrl { get; set; }
        public decimal Value { get; set; }
        public decimal WeeklyWearOff { get; set; }
        public decimal MaxWearOff { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveUntil { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
    }
}
