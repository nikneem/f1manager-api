using System;
using System.Collections.Generic;
using System.Linq;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;

namespace F1Manager.Admin.Results.DomainModels
{
    public class RaceResult : DomainModel<Guid>
    {

        private readonly  List<RaceDriverResult> _driverResults;

        public Guid RaceWeekendId { get; set; }
        public Guid FastestLapDriver { get; set; }
        public bool HasHalfPoints { get; set; }
        public IReadOnlyCollection<RaceDriverResult> DriverResults => _driverResults.AsReadOnly();

        public void AddDriverResult(RaceDriverResult model)
        {
            if (model.TrackingState != TrackingState.New)
            {
                throw new Exception("The tracking state of this race driver result is not new");
            }
            if (DriverResults.Any(x => x.DriverId == model.DriverId))
            {
                throw new Exception("This driver is already in the result set");
            }
            if (DriverResults.Count(x => x.EngineId == model.EngineId)>=2)
            {
                throw new Exception("This engine is already twice in the result set");
            }
            if (DriverResults.Count(x => x.ChassisId == model.ChassisId) >= 2)
            {
                throw new Exception("This chassis is already twice in the result set");
            }
            _driverResults.Add(model);
        }


        public RaceResult(Guid id, Guid raceWeekendId, Guid fastestLap, bool halfPointsRace,
            List<RaceDriverResult> driverResults) : base(id)
        {
            RaceWeekendId = raceWeekendId;
            FastestLapDriver = fastestLap;
            HasHalfPoints = halfPointsRace;
            _driverResults = driverResults ?? new List<RaceDriverResult>();
        }

        public RaceResult(Guid raceWeekendId, Guid fastestLap, bool halfPointsRace) : base(Guid.NewGuid(),
            TrackingState.New)
        {
            RaceWeekendId = raceWeekendId;
            FastestLapDriver = fastestLap;
            HasHalfPoints = halfPointsRace;
            _driverResults = new List<RaceDriverResult>();
        }
    }
}