using System;
using F1Manager.SqlData.Entities;
using F1Manager.Teams.Domain;

namespace F1Manager.Teams.Mappings
{
   public static  class TeamEngineMapping
    {

        public static TeamEngine ToDomainModel(this TeamEngineEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TeamEngine(
                entity.Id,
                entity.EngineId,
                entity.BoughtFor,
                entity.SoldFor,
                entity.PointsGained,
                entity.WarnOffPercentage,
                entity.BoughtOn,
                entity.SoldOn);
        }

        public static TeamEngineEntity ToEntity(this TeamEngine domainModel, Guid teamId, TeamEngineEntity existingEntity = null)
        {
            var entity = existingEntity ?? new TeamEngineEntity
            {
                Id = domainModel.Id,
                TeamId =  teamId,
                EngineId = domainModel.EngineId,
                BoughtFor = domainModel.BoughtFor,
                BoughtOn = domainModel.BoughtOn,
            };
            entity.SoldFor = domainModel.SoldFor;
            entity.SoldOn = domainModel.SoldOn;
            entity.PointsGained = domainModel.TotalPointsGained;
            entity.WarnOffPercentage = domainModel.WarnOffPercentage;
            return entity;
        }

    }
}
