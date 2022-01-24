using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Admin.Chassises.Abstractions;
using F1Manager.Admin.Chassises.DataTransferObjects;
using F1Manager.Admin.Chassises.DomainModels;
using F1Manager.Admin.Chassises.Entities;
using F1Manager.Admin.Configuration;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Admin.Chassises.Repositories
{
    public sealed class ChassisesRepository : IChassisesRepository
    {

        private const string TableName = "Components";
        private const string PartitionKey = "chassis";

        private readonly CloudTable _table;

        public async Task<List<ChassisDetailsDto>> GetActive()
        {
            var now = DateTimeOffset.UtcNow;
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(ChassisEntity.PartitionKey),
                QueryComparisons.Equal, PartitionKey);
            var availableFilter =
                TableQuery.GenerateFilterConditionForBool(nameof(ChassisEntity.IsAvailable), QueryComparisons.Equal,
                    true);
            var deletedFilter =
                TableQuery.GenerateFilterConditionForBool(nameof(ChassisEntity.IsDeleted), QueryComparisons.Equal,
                    false);
            var activeFromFilter = TableQuery.GenerateFilterConditionForDate(nameof(ChassisEntity.ActiveFrom),
                QueryComparisons.LessThanOrEqual, now);

            var filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And,
                TableQuery.CombineFilters(availableFilter, TableOperators.And,
                    TableQuery.CombineFilters(deletedFilter, TableOperators.And, activeFromFilter)));


            var query = new TableQuery<ChassisEntity>().Where(filter);

            TableContinuationToken ct = null;
            var driverEntities = new List<ChassisEntity>();
            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, ct);
                driverEntities.AddRange(segment.Results.Where(ent =>
                    !ent.ActiveUntil.HasValue || ent.ActiveUntil.Value > now));
                ct = segment.ContinuationToken;
            } while (ct != null);

            return driverEntities.Select(ToDetailsDto).ToList();
        }

        public async Task<List<ChassisDetailsDto>> GetList(ChassisListFilterDto filter)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(ChassisEntity.PartitionKey),
                QueryComparisons.Equal, PartitionKey);
            var query = new TableQuery<ChassisEntity>().Where(partitionKeyFilter);

            TableContinuationToken ct = null;
            var driverEntities = new List<ChassisEntity>();
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

        public async Task<Chassis> Get(Guid id)
        {
            var entity = await GetEntityById(id);
            return new Chassis(Guid.Parse(entity.RowKey),
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

        public async Task<ChassisDetailsDto> GetById(Guid id)
        {
            var entity = await GetEntityById(id);
            return ToDetailsDto(entity);
        }

        public async Task<bool> Create(Chassis domainModel)
        {
            if (domainModel.TrackingState == TrackingState.New)
            {
                var operation = TableOperation.Insert(ToEntity(domainModel));
                var result = await _table.ExecuteAsync(operation);
                return result.HttpStatusCode.IsSuccess();
            }

            return false;
        }

        public async Task<bool> Update(Chassis domainModel)
        {
            if (domainModel.TrackingState == TrackingState.Modified)
            {
                var operation = TableOperation.Replace(ToEntity(domainModel));
                var result = await _table.ExecuteAsync(operation);
                return result.HttpStatusCode.IsSuccess();
            }

            return false;
        }


        private async Task<ChassisEntity> GetEntityById(Guid id)
        {
            var operation = TableOperation.Retrieve<ChassisEntity>(PartitionKey, id.ToString());
            var result = await _table.ExecuteAsync(operation);
            if (result.Result is ChassisEntity entity)
            {
                return entity;
            }

            return null;
        }

        private static ChassisEntity ToEntity(Chassis domainModel)
        {
            return new ChassisEntity()
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

        private static ChassisDetailsDto ToDetailsDto(ChassisEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new ChassisDetailsDto
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


        public ChassisesRepository(IOptions<AdminOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount =
                TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }

    }
}