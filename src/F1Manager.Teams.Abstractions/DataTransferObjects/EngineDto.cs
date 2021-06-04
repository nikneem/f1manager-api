using System;

namespace F1Manager.Teams.DataTransferObjects
{
    public sealed class EngineDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public decimal Value { get; set; }
        public string PictureUrl { get; set; }
        public decimal WeeklyWearDown { get; set; }
        public decimal MaximumWearDown { get; set; }

    }
}
