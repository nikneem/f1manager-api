namespace F1Manager.Shared.Events.RaceResults;

public class PointsCalculationCommand
{
    public Guid RaceEventId { get; set; }
    public Guid DriverId { get; set; }
    public Guid EngineId { get; set; }
    public Guid ChassisId { get; set; }
    public int Qualification { get; set; }
    public int Race { get; set; }
    public int? SprintRace { get; set; }
    public bool IsFinished{ get; set; }
    public bool HasFastestLap { get; set; }
}