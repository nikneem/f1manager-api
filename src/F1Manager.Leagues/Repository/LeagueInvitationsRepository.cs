using System;
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

public class LeagueInvitationsRepository : ILeagueInvitationsRepository
{
    private const string TableName = "LeagueInvitations";

    private readonly CloudTable _table;

    public async Task<LeagueInvitation> Get(Guid leagueId, Guid teamId)
    {
        var entity = await GetEntityById(leagueId, teamId);
        if (entity != null)
        {
            return new LeagueInvitation(
                Guid.Parse(entity.PartitionKey),
                Guid.Parse(entity.RowKey),
                entity.AcceptedOn,
                entity.DeclinedOn,
                entity.CreatedOn,
                entity.ExpiresOn);
        }

        return null;
    }

    public async Task<bool> Create(LeagueInvitation domainModel)
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
    public async Task<bool> Update(LeagueInvitation domainModel)
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


    public LeagueInvitationsRepository(IOptions<LeaguesOptions> config)
    {
        var storageAccountConnectionString = config.Value.AzureStorageAccount;
        var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
        var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
        _table = tableClient.GetTableReference(TableName);
    }

}