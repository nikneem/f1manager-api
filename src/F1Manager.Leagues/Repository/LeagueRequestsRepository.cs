using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Leagues.Abstractions;
using F1Manager.Leagues.Configuration;
using F1Manager.Leagues.DomainModels;
using F1Manager.Leagues.Entities;
using F1Manager.Leagues.Mappings;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Leagues.Repository;

public class LeagueRequestsRepository : ILeagueRequestsRepository
{
    private const string TableName = "LeagueRequests";

    private readonly CloudTable _table;

    public async Task<List<LeagueRequest>> List(Guid leagueId)
    {
        var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(LeagueInvitationEntity.PartitionKey), QueryComparisons.Equal, leagueId.ToString());
        var expirationFilter = TableQuery.GenerateFilterConditionForDate(nameof(LeagueInvitationEntity.ExpiresOn), QueryComparisons.GreaterThan, DateTimeOffset.UtcNow);
        var finalFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, expirationFilter);

        var query = new TableQuery<LeagueInvitationEntity>().Where(finalFilter);

        TableContinuationToken ct = null;
        var driverEntities = new List<LeagueInvitationEntity>();
        do
        {
            var segment = await _table.ExecuteQuerySegmentedAsync(query, ct);
            driverEntities.AddRange(segment.Results);
            ct = segment.ContinuationToken;
        } while (ct != null);

        return driverEntities.Where(ent => !ent.AcceptedOn.HasValue && !ent.DeclinedOn.HasValue).Select(ent => new LeagueRequest(
            Guid.Parse(ent.PartitionKey),
            Guid.Parse(ent.RowKey),
            ent.AcceptedOn,
            ent.DeclinedOn,
            ent.CreatedOn,
            ent.ExpiresOn))
            .ToList();
    }
    public async Task<LeagueRequest> Get(Guid leagueId, Guid teamId)
    {
        var entity = await GetEntityById(leagueId, teamId);
        if (entity != null)
        {
            return new LeagueRequest(
                Guid.Parse(entity.PartitionKey),
                Guid.Parse(entity.RowKey),
                entity.AcceptedOn,
                entity.DeclinedOn,
                entity.CreatedOn,
                entity.ExpiresOn);
        }

        return null;
    }


    public async Task<bool> Create(LeagueRequest domainModel)
    {
        if (domainModel.TrackingState == TrackingState.New)
        {
            var leagueMemberEntity = domainModel.ToEntity();
            var insertOperation = TableOperation.Insert(leagueMemberEntity);
            var result = await _table.ExecuteAsync(insertOperation);
            return result.HttpStatusCode.IsSuccess();
        }
        return false;
    }
    public async Task<bool> Update(LeagueRequest domainModel)
    {
        TableOperation tableOperation = null;

        var leagueEntity = domainModel.ToEntity();
        if (domainModel.TrackingState == TrackingState.Modified)
        {
            tableOperation = TableOperation.Replace(leagueEntity);
        }

        if (domainModel.TrackingState == TrackingState.Deleted)
        {
            tableOperation = TableOperation.Delete(leagueEntity);
        }
        var result = await _table.ExecuteAsync(tableOperation);
        return result.HttpStatusCode.IsSuccess();
    }

    private async Task<LeagueInvitationEntity> GetEntityById(Guid leagueId, Guid teamId)
    {
        var operation = TableOperation.Retrieve<LeagueInvitationEntity>(leagueId.ToString(), teamId.ToString());
        var result = await _table.ExecuteAsync(operation);
        if (result.Result is LeagueInvitationEntity entity)
        {
            return entity;
        }

        return null;
    }
    public LeagueRequestsRepository(IOptions<LeaguesOptions> config)
    {
        var storageAccountConnectionString = config.Value.AzureStorageAccount;
        var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
        var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
        _table = tableClient.GetTableReference(TableName);
    }

}