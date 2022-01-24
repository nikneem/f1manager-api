using System;

namespace F1Manager.Teams.DataTransferObjects
{
    public class TeamEngineDetailsDto
    {
        public Guid Id { get; set; }
        public DateTimeOffset BoughtOn { get; set; }
        public decimal BoughtFor { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal WearOff { get; set; }
        public decimal MaxWearOff { get; set; }
        public int PointsGained { get; set; }

    }
}
