using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Admin.ActualTeams.Abstractions;
using F1Manager.Admin.ActualTeams.DataTransferObjects;
using F1Manager.Admin.ActualTeams.DomainModel;
using F1Manager.Admin.ActualTeams.Entities;
using F1Manager.Admin.Configuration;
using F1Manager.Admin.Engines.DataTransferObjects;
using F1Manager.Admin.Engines.DomainModels;
using F1Manager.Admin.Engines.Entities;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Azure.Documents;
using Microsoft.Extensions.Options;

namespace F1Manager.Admin.ActualTeams.Repositories;

public class ActualTeamsRepository:IActualTeamsRepository
{
    private const string TableName = "Components";
    private const string PartitionKey = "actualteams";

    private readonly CloudTable _table;

    public async Task<List<ActualTeamDetailsDto>> GetActive()
    {
        var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(ActualTeamEntity.PartitionKey),
            QueryComparisons.Equal, PartitionKey);
        var availableFilter =
            TableQuery.GenerateFilterConditionForBool(nameof(ActualTeamEntity.IsAvailable), QueryComparisons.Equal,
                true);
        var deletedFilter =
            TableQuery.GenerateFilterConditionForBool(nameof(ActualTeamEntity.IsDeleted), QueryComparisons.Equal,
                false);

        var filter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And,
            TableQuery.CombineFilters(availableFilter, TableOperators.And, deletedFilter));

        var query = new TableQuery<ActualTeamEntity>().Where(filter);

        TableContinuationToken ct = null;
        var actualTeamsEntities = new List<ActualTeamEntity>();
        do
        {
            var segment = await _table.ExecuteQuerySegmentedAsync(query, ct);
            actualTeamsEntities.AddRange(segment.Results);
            ct = segment.ContinuationToken;
        } while (ct != null);

        return actualTeamsEntities.Select(ToDetailsDto).ToList();
    }

    public async Task<List<ActualTeamDetailsDto>> GetList(ActualTeamListFilterDto filter)
    {
        var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(ActualTeamEntity.PartitionKey),
            QueryComparisons.Equal, PartitionKey);
        var query = new TableQuery<ActualTeamEntity>().Where(partitionKeyFilter);

        TableContinuationToken ct = null;
        var actualTeamsEntities = new List<ActualTeamEntity>();
        do
        {
            var segment = await _table.ExecuteQuerySegmentedAsync(query, ct);
            actualTeamsEntities.AddRange(segment.Results);
            ct = segment.ContinuationToken;
        } while (ct != null);

        if (!string.IsNullOrWhiteSpace(filter.Name))
        {
            actualTeamsEntities = actualTeamsEntities.Where(x =>
                x.Name.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase)
            ).ToList();
        }

        if (!filter.Deleted)
        {
            actualTeamsEntities = actualTeamsEntities.Where(x => !x.IsDeleted).ToList();
        }

        return actualTeamsEntities.Select(ToDetailsDto).ToList();
    }

    public async  Task<ActualTeam> Get(Guid id)
    {
        var entity = await GetEntityById(id);
        return new ActualTeam(Guid.Parse(entity.RowKey),
            entity.Name,
            entity.Base,
            entity.Principal,
            entity.TechnicalChief,
            entity.FirstDriverId,
            entity.SecondDriverId,
            entity.EngineId,
            entity.ChassisId,
            entity.IsAvailable,
            entity.IsDeleted);
    }

    public async Task<ActualTeamDetailsDto> GetById(Guid id)
    {
        var entity = await GetEntityById(id);
        return ToDetailsDto(entity);
    }

    public async Task<bool> Create(ActualTeam domainModel)
    {
        if (domainModel.TrackingState == TrackingState.New)
        {
            var operation = TableOperation.Insert(ToEntity(domainModel));
            var result = await _table.ExecuteAsync(operation);
            return result.HttpStatusCode.IsSuccess();
        }

        return false;
    }

    public async Task<bool> Update(ActualTeam domainModel)
    {
        if (domainModel.TrackingState == TrackingState.Modified)
        {
            var operation = TableOperation.Replace(ToEntity(domainModel));
            var result = await _table.ExecuteAsync(operation);
            return result.HttpStatusCode.IsSuccess();
        }

        return false;
    }

    private async Task<ActualTeamEntity> GetEntityById(Guid id)
    {
        var operation = TableOperation.Retrieve<ActualTeamEntity>(PartitionKey, id.ToString());
        var result = await _table.ExecuteAsync(operation);
        if (result.Result is ActualTeamEntity entity)
        {
            return entity;
        }

        return null;
    }

    private static ActualTeamDetailsDto ToDetailsDto(ActualTeamEntity entity)
    {
        if (entity == null)
        {
            return null;
        }

        return new ActualTeamDetailsDto
        {
            Id = Guid.Parse(entity.RowKey),
            Name = entity.Name,
            Base = entity.Base,
            Principal = entity.Principal,
            TechnicalChief = entity.TechnicalChief,
            FirstDriverId = entity.FirstDriverId,
            SecondDriverId = entity.SecondDriverId,
            EngineId = entity.EngineId,
            ChassisId = entity.ChassisId,
            IsAvailable = entity.IsAvailable,
            IsDeleted = entity.IsDeleted
        };
    }

    private static ActualTeamEntity ToEntity(ActualTeam domainModel)
    {
        return new ActualTeamEntity()
        {
            PartitionKey = PartitionKey,
            RowKey = domainModel.Id.ToString(),
            Name = domainModel.Name,
            Base = domainModel.Base,
            Principal = domainModel.Principal,
            TechnicalChief = domainModel.TechnicalChief,
            FirstDriverId = domainModel.FirstDriverId,
            SecondDriverId = domainModel.SecondDriverId,
            EngineId = domainModel.EngineId,
            ChassisId = domainModel.ChassisId,
            IsAvailable = domainModel.IsAvailable,
            IsDeleted = domainModel.IsDeleted,
            ETag = "*"
        };
    }



    public ActualTeamsRepository(IOptions<AdminOptions> config)
    {
        var storageAccountConnectionString = config.Value.AzureStorageAccount;
        var storageAccount =
            TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
        var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
        _table = tableClient.GetTableReference(TableName);
    }
}










