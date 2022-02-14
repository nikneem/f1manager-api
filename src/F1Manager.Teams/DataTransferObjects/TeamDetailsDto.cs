using System;

namespace F1Manager.Teams.DataTransferObjects
{
    public sealed class TeamDetailsDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public decimal Money { get; set; }
        public bool IsPublic { get; set; }
        public bool IsTeamOwner { get; set; }
        public Guid? FirstDriverId { get; set; }
        public Guid? SecondDriverId { get; set; }
        public Guid? EngineId { get; set; }
        public Guid? ChassisId { get; set; }
    }
}
