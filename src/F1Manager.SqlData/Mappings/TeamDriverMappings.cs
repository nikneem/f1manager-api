using System;
using System.Linq;
using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class TeamDriverMappings
    {

        public static TeamDriver ToDomainModel(this TeamDriverEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            if (entity.Driver == null)
            {
                throw new NullReferenceException("The engine property is not set to an instance of an object");
            }

            var driver = entity.Driver.ToDomainModel();
            var history = entity.History.Select(x => new TeamDriverHistory {HistoryCreatedOn = x.HistoryCreatedOn})
                .ToList();
            return new TeamDriver(entity.Id,
                driver,
                entity.PurchasePrice,
                entity.SellingPrice,
                entity.IsFirstDriver,
                entity.BoughtOn,
                entity.SoldOn,
                history);
        }

        public static TeamDriverEntity ToEntity(this TeamDriver domainModel, Guid teamId, TeamDriverEntity existingEntity = null)
        {
            var entity = existingEntity ?? new TeamDriverEntity
            {
                Id = domainModel.Id,
                TeamId =  teamId,
                DriverId = domainModel.Driver.Id,
                PurchasePrice = domainModel.PurchasePrice,
                BoughtOn = domainModel.BoughtOn
            };
            entity.SellingPrice = domainModel.SellingPrice;
            entity.SoldOn = domainModel.SoldOn;

            return entity;
        }


    }
}
