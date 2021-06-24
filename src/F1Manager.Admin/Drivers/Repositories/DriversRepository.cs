using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using F1Manager.Admin.Configuration;
using F1Manager.Admin.Drivers.Abstractions;
using F1Manager.Admin.Drivers.DataTransferObjects;
using F1Manager.Admin.Drivers.DomainModels;
using F1Manager.Admin.Drivers.Entities;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Admin.Drivers.Repositories
{
    public sealed class DriversRepository : IDriversRepository
    {

        private const string TableName = "Components";
        private const string PartitionKey = "driver";

        private readonly CloudTable _table;

        public async Task<List<DriverDetailsDto>> GetActive()
        {
            var now = DateTimeOffset.UtcNow;
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(DriverEntity.PartitionKey), QueryComparisons.Equal, PartitionKey);
            var availableFilter = TableQuery.GenerateFilterConditionForBool(nameof(DriverEntity.IsAvailable), QueryComparisons.Equal, true);
            var deletedFilter = TableQuery.GenerateFilterConditionForBool(nameof(DriverEntity.IsDeleted), QueryComparisons.Equal, false);
            var activeFromFilter = TableQuery.GenerateFilterConditionForDate(nameof(DriverEntity.ActiveFrom), QueryComparisons.LessThanOrEqual, now);

            var filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And,
                TableQuery.CombineFilters(availableFilter, TableOperators.And,
                    TableQuery.CombineFilters(deletedFilter, TableOperators.And, activeFromFilter)));
            
            
            var query = new TableQuery<DriverEntity>().Where(filter);

            TableContinuationToken ct = null;
            var driverEntities = new List<DriverEntity>();
            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, ct);
                driverEntities.AddRange(segment.Results.Where(ent => !ent.ActiveUntil.HasValue || ent.ActiveUntil.Value > now));
                ct = segment.ContinuationToken;
            } while (ct != null);

            return driverEntities.Select(ToDetailsDto).ToList();
        }

        public async Task<List<DriverDetailsDto>> GetList(DriversListFilterDto filter)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(DriverEntity.PartitionKey), QueryComparisons.Equal, PartitionKey);
            var query = new TableQuery<DriverEntity>().Where(partitionKeyFilter);

            TableContinuationToken ct = null;
            var driverEntities = new List<DriverEntity>();
            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, ct);
                driverEntities.AddRange(segment.Results);
                ct = segment.ContinuationToken;
            } while (ct != null);

            if (!string.IsNullOrWhiteSpace(filter.Name))
            {
                driverEntities = driverEntities.Where(x =>
                    x.Name.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase)
                ).ToList();
            }
            if (!filter.IncludeDeleted)
            {
                driverEntities = driverEntities.Where(x => !x.IsDeleted).ToList();
            }

            return driverEntities.Select(ToDetailsDto).ToList();
        }

        public async Task<Driver> Get(Guid id)
        {
            var entity = await GetEntityById(id);
            return new Driver(Guid.Parse(entity.RowKey),
                entity.Name,
                entity.DateOfBirth,
                entity.Country,
                entity.Value,
                entity.PictureUrl,
                entity.ActiveFrom,
                entity.ActiveUntil,
                entity.IsAvailable,
                entity.IsDeleted);
        }

        public async Task<DriverDetailsDto> GetById(Guid id)
        {
            var entity = await GetEntityById(id);
            return ToDetailsDto(entity);
        }
        public async Task<bool> Create(Driver domainModel)
        {
            if (domainModel.TrackingState == TrackingState.New)
            {
                var operation = TableOperation.Insert(ToEntity(domainModel));
                var result = await _table.ExecuteAsync(operation);
                return result.HttpStatusCode.IsSuccess();
            }
            return false;
        }
        public async Task<bool> Update(Driver domainModel)
        {
            if (domainModel.TrackingState == TrackingState.Modified)
            {
                var operation = TableOperation.Replace(ToEntity(domainModel));
                var result = await _table.ExecuteAsync(operation);
                return result.HttpStatusCode.IsSuccess();
            }
            return false;
        }


        private async Task<DriverEntity> GetEntityById(Guid id)
        {
            var operation = TableOperation.Retrieve<DriverEntity>(PartitionKey, id.ToString());
            var result = await _table.ExecuteAsync(operation);
            if (result.Result is DriverEntity entity)
            {
                return entity;
            }

            return null;
        }
        private static DriverEntity ToEntity(Driver domainModel)
        {
            return new DriverEntity
            {
                PartitionKey = PartitionKey,
                RowKey = domainModel.Id.ToString(),
                Name = domainModel.Name,
                Country = domainModel.Country,
                DateOfBirth = domainModel.DateOfBirth,
                PictureUrl = domainModel.PictureUrl,
                Value = domainModel.Value,
                ActiveFrom = domainModel.ActiveFrom,
                ActiveUntil = domainModel.ActiveUntil,
                IsAvailable = domainModel.IsAvailable,
                IsDeleted = domainModel.IsDeleted,
                ETag = "*"
            };
        }
        private static DriverDetailsDto ToDetailsDto(DriverEntity entity)
        {
            if (entity == null)
            {
                return null;
            }
            return new DriverDetailsDto
            {
                Id = Guid.Parse(entity.RowKey),
                Name = entity.Name,
                Country = entity.Country,
                DateOfBirth = entity.DateOfBirth,
                PictureUrl = entity.PictureUrl,
                Value = entity.Value,
                ActiveFrom = entity.ActiveFrom,
                ActiveUntil = entity.ActiveUntil,
                IsAvailable = entity.IsAvailable,
                IsDeleted = entity.IsDeleted,
            };
        }


        public DriversRepository(IOptions<AdminOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }

    }
}
