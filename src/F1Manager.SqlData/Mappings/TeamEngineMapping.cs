using System;
using System.Linq;
using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
   public static  class TeamEngineMapping
    {

        public static TeamEngine ToDomainModel(this TeamEngineEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            if (entity.Engine == null)
            {
                throw new NullReferenceException("The engine property is not set to an instance of an object");
            }

            var engine = entity.Engine.ToDomainModel();
            var history = entity.History.Select(x => new TeamEngineHistory {HistoryCreatedOn = x.HistoryCreatedOn})
                .ToList();
            return new TeamEngine(entity.Id,
                engine,
                entity.PurchasePrice,
                entity.SellingPrice,
                entity.BoughtOn,
                entity.SoldOn,
                history);
        }

        public static TeamEngineEntity ToEntity(this TeamEngine domainModel, Guid teamId, TeamEngineEntity existingEntity = null)
        {
            var entity = existingEntity ?? new TeamEngineEntity
            {
                Id = domainModel.Id,
                TeamId =  teamId,
                EngineId = domainModel.Engine.Id,
                PurchasePrice = domainModel.PurchasePrice,
                BoughtOn = domainModel.BoughtOn
            };
            entity.SellingPrice = domainModel.SellingPrice;
            entity.SoldOn = domainModel.SoldOn;

            return entity;
        }

    }
}
