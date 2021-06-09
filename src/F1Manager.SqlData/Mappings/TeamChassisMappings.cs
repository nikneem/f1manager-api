using System;
using System.Linq;
using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class TeamChassisMappings
    {

        public static TeamChassis ToDomainModel(this TeamChassisEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            if (entity.Chassis == null)
            {
                throw new NullReferenceException("The engine property is not set to an instance of an object");
            }

            var chassis = entity.Chassis.ToDomainModel();
            var history = entity.History.Select(x => new TeamChassisHistory {HistoryCreatedOn = x.HistoryCreatedOn})
                .ToList();
            return new TeamChassis(entity.Id,
                chassis,
                entity.PurchasePrice,
                entity.SellingPrice,
                entity.BoughtOn,
                entity.SoldOn,
                history);
        }

        public static TeamChassisEntity ToEntity(this TeamChassis domainModel, Guid teamId, TeamChassisEntity existingEntity = null)
        {
            var entity = existingEntity ?? new TeamChassisEntity
            {
                Id = domainModel.Id,
                TeamId =  teamId,
                ChassisId = domainModel.Chassis.Id,
                PurchasePrice = domainModel.PurchasePrice,
                BoughtOn = domainModel.BoughtOn
            };
            entity.SellingPrice = domainModel.SellingPrice;
            entity.SoldOn = domainModel.SoldOn;

            return entity;
        }

    }
}
