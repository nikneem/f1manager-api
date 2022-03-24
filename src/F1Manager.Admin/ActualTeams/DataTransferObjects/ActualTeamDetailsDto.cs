using System;

namespace F1Manager.Admin.ActualTeams.DataTransferObjects;

public class ActualTeamDetailsDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Base { get; set; }
    public string Principal { get; set; }
    public string TechnicalChief { get; set; }
    public Guid FirstDriverId { get; set; }
    public Guid SecondDriverId { get; set; }
    public Guid ChassisId { get; set; }
    public Guid EngineId { get; set; }
    public bool IsAvailable { get; set; }
}