using System;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;

namespace F1Manager.Admin.Results.DomainModels
{
    public class RaceDriverResult : DomainModel<Guid>
    {
        public Guid RaceWeekendId { get; set; }
        public Guid DriverId { get; set; }
        public Guid EngineId { get; set; }
        public Guid ChassisId { get; set; }
        public int QualificationResult { get; set; }
        public int? SprintRaceResult { get; set; }
        public int RaceResult { get; set; }
        public bool IsFinished { get; set; }

        public RaceDriverResult(Guid id, Guid driverId, int quali, int? sprint, int race, bool finished, Guid engine,
            Guid chassis) : base(id)
        {
            DriverId = driverId;
            QualificationResult = quali;
            SprintRaceResult = sprint;
            RaceResult = race;
            IsFinished = finished;
            EngineId = engine;
            ChassisId = chassis;
        }

        public RaceDriverResult(Guid driverId, int quali, int? sprint, int race, bool finished, Guid engine, Guid chassis) : base(Guid.NewGuid(), TrackingState.New)
        {
            DriverId = driverId;
            QualificationResult = quali;
            SprintRaceResult = sprint;
            RaceResult = race;
            IsFinished = finished;
            EngineId = engine;
            ChassisId = chassis;
        }
    }
}
