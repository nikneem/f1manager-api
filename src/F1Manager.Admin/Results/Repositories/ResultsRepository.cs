using System;
using System.Threading.Tasks;
using F1Manager.Admin.Results.DomainModels;
using F1Manager.Shared.Enums;
using F1Manager.SqlData;
using F1Manager.SqlData.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace F1Manager.Admin.Results.Repositories
{
    public class ResultsRepository
    {
        private readonly DbContextOptions _dbContextOptions;
        private readonly ILogger<ResultsRepository> _logger;

        public async Task<bool> Create(RaceResult domainModel)
        {
            bool success = false;
            await using var context = new F1ManagerDbContext(_dbContextOptions);
            if (domainModel.TrackingState == TrackingState.New)
            {
                var entity = ToEntity(domainModel);
                context.Add(entity);
                foreach (var driverResult in domainModel.DriverResults)
                {
                    var driverEntity = ToEntity(driverResult);
                    context.Add(driverEntity);
                }

                success = await context.SaveChangesAsync() > 0;
            }

            return success;
        }

        private static RaceResultEntity ToEntity(RaceResult domainModel, RaceResultEntity existing = null)
        {
            var entity = existing ?? new RaceResultEntity
            {
                Id = domainModel.Id,
                RaceWeekendId = domainModel.RaceWeekendId,
                CreatedOn = DateTimeOffset.UtcNow
            };
            entity.FastestLapDriverId = domainModel.FastestLapDriver;
            entity.HasHalfPoints = domainModel.HasHalfPoints;
            entity.ModifiedOn = DateTimeOffset.UtcNow;
            return entity;
        }
        private static RaceDriverResultsEntity ToEntity(RaceDriverResult domainModel, RaceDriverResultsEntity existing = null)
        {
            var entity = existing ?? new RaceDriverResultsEntity
            {
                Id = domainModel.Id,
                RaceWeekendId = domainModel.RaceWeekendId,
                CreatedOn = DateTimeOffset.UtcNow
            };
            entity.DriverId = domainModel.DriverId;
            entity.EngineId = domainModel.EngineId;
            entity.ChassisId = domainModel.ChassisId;
            entity.QualificationResult = domainModel.QualificationResult;
            entity.RaceResult = domainModel.RaceResult;
            entity.SprintRaceResult = domainModel.SprintRaceResult;
            entity.IsFinished = domainModel.IsFinished;
            entity.ModifiedOn = DateTimeOffset.UtcNow;
            return entity;
        }


        public ResultsRepository(DbContextOptions dbContextOptions,
            ILogger<ResultsRepository> logger)
        {
            _dbContextOptions = dbContextOptions;
            _logger = logger;
        }
    }
}