using System;
using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class RaceResultMappings
    {

        public static RaceResultEntity ToEntity(this RaceResult model, RaceResultEntity existing = null)
        {
            var entity = existing ?? new RaceResultEntity {Id = model.Id};
            entity.RaceId = model.RaceId;
            entity.FastestLapDriver = model.FastestLapDriver;
            entity.WinningTireStrategy = string.Join(",", model.WinningTireStrategy);
            return entity;
        }
        public static RaceDriverResultEntity ToEntity(this RaceDriverResult model, Guid raceId, RaceDriverResultEntity existing = null)
        {
            var entity = existing ?? new RaceDriverResultEntity {Id = model.Id};
            entity.RaceId = raceId;
            entity.DriverId = model.DriverId;
            entity.QualificationPosition = model.QualificationPosition;
            entity.RacePosition= model.RacePosition;
            entity.IsFinished= model.IsFinished;
            return entity;
        }
    }
}
