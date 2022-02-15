using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Admin.Configuration;
using F1Manager.Admin.Engines.Abstractions;
using F1Manager.Admin.Engines.DataTransferObjects;
using F1Manager.Admin.Engines.DomainModels;
using F1Manager.Admin.Engines.Entities;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Admin.Engines.Repositories
{
    public sealed class EnginesRepository : IEnginesRepository
    {

        private const string TableName = "Components";
        private const string PartitionKey = "engine";

        private readonly CloudTable _table;

        public async Task<List<EngineDetailsDto>> GetActive()
        {
            var now = DateTimeOffset.UtcNow;
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(EngineEntity.PartitionKey),
                QueryComparisons.Equal, PartitionKey);
            var availableFilter =
                TableQuery.GenerateFilterConditionForBool(nameof(EngineEntity.IsAvailable), QueryComparisons.Equal,
                    true);
            var deletedFilter =
                TableQuery.GenerateFilterConditionForBool(nameof(EngineEntity.IsDeleted), QueryComparisons.Equal,
                    false);
            var activeFromFilter = TableQuery.GenerateFilterConditionForDate(nameof(EngineEntity.ActiveFrom),
                QueryComparisons.LessThanOrEqual, now);

            var filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And,
                TableQuery.CombineFilters(availableFilter, TableOperators.And,
                    TableQuery.CombineFilters(deletedFilter, TableOperators.And, activeFromFilter)));


            var query = new TableQuery<EngineEntity>().Where(filter);

            TableContinuationToken ct = null;
            var driverEntities = new List<EngineEntity>();
            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, ct);
                driverEntities.AddRange(segment.Results.Where(ent =>
                    !ent.ActiveUntil.HasValue || ent.ActiveUntil.Value > now));
                ct = segment.ContinuationToken;
            } while (ct != null);

            return driverEntities.Select(ToDetailsDto).ToList();
        }

        public async Task<List<EngineDetailsDto>> GetList(EnginesListFilterDto filter)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(EngineEntity.PartitionKey),
                QueryComparisons.Equal, PartitionKey);
            var query = new TableQuery<EngineEntity>().Where(partitionKeyFilter);

            TableContinuationToken ct = null;
            var driverEntities = new List<EngineEntity>();
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

            if (!filter.Deleted)
            {
                driverEntities = driverEntities.Where(x => !x.IsDeleted).ToList();
            }

            return driverEntities.Select(ToDetailsDto).ToList();
        }

        public async Task<Engine> Get(Guid id)
        {
            var entity = await GetEntityById(id);
            return new Engine(Guid.Parse(entity.RowKey),
                entity.Name,
                entity.Manufacturer,
                entity.Model,
                entity.PictureUrl,
                Convert.ToDecimal(entity.Value),
                Convert.ToDecimal(entity.WeeklyWearOff),
                Convert.ToDecimal(entity.MaxWearOff),
                entity.ActiveFrom,
                entity.ActiveUntil,
                entity.IsAvailable,
                entity.IsDeleted);
        }

        public async Task<EngineDetailsDto> GetById(Guid id)
        {
            var entity = await GetEntityById(id);
            return ToDetailsDto(entity);
        }

        public async Task<bool> Create(Engine domainModel)
        {
            if (domainModel.TrackingState == TrackingState.New)
            {
                var operation = TableOperation.Insert(ToEntity(domainModel));
                var result = await _table.ExecuteAsync(operation);
                return result.HttpStatusCode.IsSuccess();
            }

            return false;
        }

        public async Task<bool> Update(Engine domainModel)
        {
            if (domainModel.TrackingState == TrackingState.Modified)
            {
                var operation = TableOperation.Replace(ToEntity(domainModel));
                var result = await _table.ExecuteAsync(operation);
                return result.HttpStatusCode.IsSuccess();
            }

            return false;
        }


        private async Task<EngineEntity> GetEntityById(Guid id)
        {
            var operation = TableOperation.Retrieve<EngineEntity>(PartitionKey, id.ToString());
            var result = await _table.ExecuteAsync(operation);
            if (result.Result is EngineEntity entity)
            {
                return entity;
            }

            return null;
        }

        private static EngineEntity ToEntity(Engine domainModel)
        {
            return new EngineEntity()
            {
                PartitionKey = PartitionKey,
                RowKey = domainModel.Id.ToString(),
                Name = domainModel.Name,
                Manufacturer = domainModel.Manufacturer,
                Model = domainModel.Model,
                WeeklyWearOff = Convert.ToDouble(domainModel.WeeklyWearOffPercentage),
                MaxWearOff = Convert.ToDouble(domainModel.MaxWearOffPercentage),
                PictureUrl = domainModel.PictureUrl,
                Value = Convert.ToDouble(domainModel.Value),
                ActiveFrom = domainModel.ActiveFrom,
                ActiveUntil = domainModel.ActiveUntil,
                IsAvailable = domainModel.IsAvailable,
                IsDeleted = domainModel.IsDeleted,
                ETag = "*"
            };
        }

        private static EngineDetailsDto ToDetailsDto(EngineEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new EngineDetailsDto
            {
                Id = Guid.Parse(entity.RowKey),
                Name = entity.Name,
                Manufacturer = entity.Manufacturer,
                Model = entity.Model,
                WeeklyWearOff = Convert.ToDecimal(entity.WeeklyWearOff),
                MaxWearOff = Convert.ToDecimal(entity.MaxWearOff),
                PictureUrl = entity.PictureUrl,
                Value = Convert.ToDecimal(entity.Value),
                ActiveFrom = entity.ActiveFrom,
                ActiveUntil = entity.ActiveUntil,
                IsAvailable = entity.IsAvailable,
                IsDeleted = entity.IsDeleted,
            };
        }


        public EnginesRepository(IOptions<AdminOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount =
                TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }

    }
}