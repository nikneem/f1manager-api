using System;
using F1Manager.SqlData.Entities;
using F1Manager.Teams.Domain;

namespace F1Manager.Teams.Mappings
{
    public static class TeamChassisMappings
    {

        public static TeamChassis ToDomainModel(this TeamChassisEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TeamChassis(
                entity.Id,
                entity.ChassisId,
                entity.Name,
                entity.PictureUrl,
                entity.Manufacturer,
                entity.Model,
                entity.BoughtFor,
                entity.SoldFor,
                entity.PointsGained,
                entity.WarnOffPercentage,
                entity.BoughtOn,
                entity.SoldOn);
        }

        public static TeamChassisEntity ToEntity(this TeamChassis domainModel, Guid teamId, TeamChassisEntity existingEntity = null)
        {
            var entity = existingEntity ?? new TeamChassisEntity
            {
                Id = domainModel.Id,
                TeamId = teamId,
                ChassisId = domainModel.ChassisId,
                BoughtFor = domainModel.BoughtFor,
                BoughtOn = domainModel.BoughtOn,
                Manufacturer = domainModel.Manufacturer,
                Model = domainModel.Model,
                Name = domainModel.Name,
                PictureUrl = domainModel.PictureUrl,

            };
            entity.SoldFor = domainModel.SoldFor;
            entity.SoldOn = domainModel.SoldOn;
            entity.PointsGained = domainModel.TotalPointsGained;
            entity.WarnOffPercentage = domainModel.WarnOffPercentage;
            return entity;
        }

    }
}
