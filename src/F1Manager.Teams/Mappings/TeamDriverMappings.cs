﻿using System;
using F1Manager.SqlData.Entities;
using F1Manager.Teams.Domain;

namespace F1Manager.Teams.Mappings
{
    public static class TeamDriverMappings
    {

        public static TeamDriver ToDomainModel(this TeamDriverEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new TeamDriver(entity.Id,
                entity.DriverId,
                entity.Name,
                entity.PictureUrl,
                entity.Country,
                entity.BirthDate,
                entity.PurchasePrice,
                entity.SellingPrice,
                entity.PointsGained,
                entity.BoughtOn,
                entity.SoldOn);
        }

        public static TeamDriverEntity ToEntity(this TeamDriver domainModel, Guid teamId, TeamDriverEntity existingEntity = null)
        {
            var entity = existingEntity ?? new TeamDriverEntity
            {
                Id = domainModel.Id,
                TeamId =  teamId,
                DriverId = domainModel.DriverId,
                PurchasePrice = domainModel.BoughtFor,
                BoughtOn = domainModel.BoughtOn,
                Country= domainModel.Country,
                BirthDate = domainModel.BirthDate,
                Name = domainModel.Name,
                PictureUrl = domainModel.PictureUrl,
            };
            entity.SellingPrice = domainModel.SoldFor;
            entity.SoldOn = domainModel.SoldOn;

            return entity;
        }


    }
}
