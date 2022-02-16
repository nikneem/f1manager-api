using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Leagues.Abstractions;
using F1Manager.Leagues.Configuration;
using F1Manager.Leagues.DataTransferObjects;
using F1Manager.Leagues.DomainModels;
using F1Manager.Leagues.Entities;
using F1Manager.Leagues.Mappings;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Leagues.Repository;

public sealed class LeaguesRepository : ILeaguesRepository
{

    private const string LeaguesTableName = "Leagues";
    public const string LeaguesPartitionKey = "league";
    private readonly CloudTable _leaguesTable;


    public async Task< List<LeagueListDto>> List(Guid teamId)
    {
        var memberships = await GetMemberships(teamId);
        var leagueIds = memberships.Select(m => Guid.Parse(m.PartitionKey)).ToList();
        var leagueEntities = await GetEntitiesByIds(leagueIds);
        return leagueEntities.Select(league => new LeagueListDto
        {
            Id = Guid.Parse(league.RowKey),
             Name = league.Name,
             CreatedOn = league.CreatedOn
        }).ToList();

    }

    public async Task<League> Get(Guid leagueId)
    {
        var memberEntities = await GetMembers(leagueId);
        var leagueEntity = await GetEntityById(leagueId);

        var memberModels = memberEntities.Select(m => new LeagueMember(Guid.Parse(m.RowKey), m.JoinedOn)).ToList();
        return new League(Guid.Parse(leagueEntity.RowKey),
            leagueEntity.OwnerId,
            leagueEntity.Name,
            leagueEntity.CreatedOn,
            memberModels);
    }
    public async Task<bool> Create(League domainModel)
    {
        if (domainModel.TrackingState == TrackingState.New)
        {
            var batchOperation = new TableBatchOperation();
            var leagueEntity = domainModel.ToEntity();
            batchOperation.Add(TableOperation.Insert(leagueEntity));

            foreach (var member in domainModel.Members.Where(x => x.TrackingState == TrackingState.New))
            {
                var memberEntity = member.ToEntity(domainModel.Id);
                batchOperation.Add(TableOperation.Insert(memberEntity));
            }

            var result = await _leaguesTable.ExecuteBatchAsync(batchOperation);
            return result.All(x => x.HttpStatusCode.IsSuccess());
        }

        return false;
    }
    public async Task<bool> Update(League domainModel)
    {
        if (domainModel.TrackingState == TrackingState.Modified || domainModel.TrackingState == TrackingState.Touched)
        {
            var batchOperation = new TableBatchOperation();
            if (domainModel.TrackingState == TrackingState.Modified)
            {
                var leagueEntity = domainModel.ToEntity();
                batchOperation.Add(TableOperation.Insert(leagueEntity));
            }

            foreach (var member in domainModel.Members.Where(x => x.TrackingState == TrackingState.New))
            {
                var memberEntity = member.ToEntity(domainModel.Id);
                batchOperation.Add(TableOperation.Insert(memberEntity));
            }
            foreach (var member in domainModel.Members.Where(x => x.TrackingState == TrackingState.Modified))
            {
                var memberEntity = member.ToEntity(domainModel.Id);
                batchOperation.Add(TableOperation.Replace(memberEntity));
            }
            foreach (var member in domainModel.Members.Where(x => x.TrackingState == TrackingState.Deleted))
            {
                var memberEntity = member.ToEntity(domainModel.Id);
                batchOperation.Add(TableOperation.Delete(memberEntity));
            }

            var result = await _leaguesTable.ExecuteBatchAsync(batchOperation);
            return result.All(x => x.HttpStatusCode.IsSuccess());
        }

        return false;
    }

    private async Task<List<LeagueEntity>> GetEntitiesByIds(List<Guid> leagueIds)
    {
        if (leagueIds.Count > 0)
        {
            var firstLeagueId = leagueIds.First();
            var baseFilter= TableQuery.GenerateFilterCondition(nameof(LeagueEntity.RowKey),
                QueryComparisons.Equal, firstLeagueId.ToString());
            foreach (var leagueId in leagueIds)
            {
                if (!Equals(leagueId, firstLeagueId))
                {
                    var additionalFilter = TableQuery.GenerateFilterCondition(nameof(LeagueEntity.RowKey),
                        QueryComparisons.Equal, firstLeagueId.ToString());
                    baseFilter = TableQuery.CombineFilters(baseFilter, TableOperators.Or, additionalFilter);
                }
            }
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(LeagueEntity.PartitionKey),
                QueryComparisons.Equal, LeaguesPartitionKey);
            var finalFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.Or, baseFilter);

            TableQuery<LeagueEntity> query = new TableQuery<LeagueEntity>().Where(finalFilter);
            var leagues = new List<LeagueEntity>();
            TableContinuationToken ct = null;
            do
            {
                var segment = await _leaguesTable.ExecuteQuerySegmentedAsync(query, ct);
                leagues.AddRange(segment.Results);
                ct = segment.ContinuationToken;
            } while (ct != null);

            return leagues;
        }

        return null;
    }
    private async Task<LeagueEntity> GetEntityById(Guid leagueId)
    {
        var operation = TableOperation.Retrieve<LeagueEntity>(LeaguesPartitionKey, leagueId.ToString());
        var result = await _leaguesTable.ExecuteAsync(operation);
        if (result.Result is LeagueEntity entity)
        {
            return entity;
        }

        return null;
    }
    private async Task<List<LeagueMemberEntity>> GetMembers(Guid leagueId)
    {
        var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(LeagueMemberEntity.PartitionKey),
            QueryComparisons.Equal, leagueId.ToString());

        TableQuery<LeagueMemberEntity> query = new TableQuery<LeagueMemberEntity>().Where(partitionKeyFilter);

        var leagueMembers = new List<LeagueMemberEntity>();
        TableContinuationToken ct = null;
        do
        {
            var segment = await _leaguesTable.ExecuteQuerySegmentedAsync(query, ct);
            leagueMembers.AddRange(segment.Results);
            ct = segment.ContinuationToken;
        } while (ct != null);

        return leagueMembers;
    }
    /// <summary>
    /// Get all the leagues this team is member of
    /// </summary>
    private async Task<List<LeagueMemberEntity>> GetMemberships(Guid teamId)
    {

        var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(LeagueMemberEntity.RowKey),
            QueryComparisons.Equal, teamId.ToString());

        TableQuery<LeagueMemberEntity> query = new TableQuery<LeagueMemberEntity>().Where(partitionKeyFilter);

        var leagueMembers = new List<LeagueMemberEntity>();
        TableContinuationToken ct = null;
        do
        {
            var segment = await _leaguesTable.ExecuteQuerySegmentedAsync(query, ct);
            leagueMembers.AddRange(segment.Results);
            ct = segment.ContinuationToken;
        } while (ct != null);

        return leagueMembers;
    }

    public LeaguesRepository(IOptions<LeaguesOptions> config)
    {
        var storageAccountConnectionString = config.Value.AzureStorageAccount;
        var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
        var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
        _leaguesTable = tableClient.GetTableReference(LeaguesTableName);
    }

}