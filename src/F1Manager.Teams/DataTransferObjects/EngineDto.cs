using System;

namespace F1Manager.Teams.DataTransferObjects
{
    public sealed class EngineDto
    {

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string Model { get; set; }
        public decimal Value { get; set; }
        public string PictureUrl { get; set; }
        public decimal WeeklyWearOff { get; set; }
        public decimal MaximumWearOff { get; set; }

    }
}
