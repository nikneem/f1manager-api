using System;
using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class BaseTeamMappings
    {
        public static BaseTeam ToDomainModel(this BaseTeamEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            var season = entity.Season.ToDomainModel();
            var fDriver = entity.FirstDriver.ToDomainModel();
            var sDriver = entity.SecondDriver.ToDomainModel();
            var engine = entity.Engine.ToDomainModel();
            var chassis = entity.Chassis.ToDomainModel();

            return new BaseTeam(entity.Id,
                season,
                entity.Name,
                entity.Origin,
                entity.Principal,
                fDriver,
                sDriver,
                engine,
                chassis,
                entity.CreatedOn,
                entity.ModifiedOn);
        }

        public static BaseTeamEntity ToEntity(this BaseTeam model, BaseTeamEntity existing = null)
        {
            var entity = existing ?? new BaseTeamEntity
            {
                Id = model.Id,
                CreatedOn = DateTimeOffset.UtcNow,
                SeasonId = model.Season.Id
            };
            entity.Name = model.Name;
            entity.Origin = model.Origin;
            entity.Principal = model.Principal;
            entity.FirstDriverId = model.FirstDriver.Id;
            entity.SecondDriverId = model.SecondDriver.Id;
            entity.EngineId = model.Engine.Id;
            entity.ChassisId = model.Chassis.Id;
            entity.ModifiedOn = DateTimeOffset.UtcNow;
            return entity;
        }
    }
}