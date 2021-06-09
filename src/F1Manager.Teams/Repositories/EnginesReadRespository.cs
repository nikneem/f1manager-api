using System;
using System.Threading.Tasks;
using F1Manager.Shared.Helpers;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.Configuration;
using F1Manager.Teams.DataTransferObjects;
using F1Manager.Teams.Entities;
using F1Manager.Teams.Enums;
using F1Manager.Teams.Exceptions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Teams.Repositories
{
    public sealed class EnginesReadRespository : IEnginesReadRespository
    {
        private const string TableName = "Components";
        public const string PartitionKey = "engine";

        private readonly CloudTable _table;

        public async Task<EngineDto> GetById(Guid id)
        {
            var operation = TableOperation.Retrieve<EngineEntity>(PartitionKey, id.ToString());
            var result = await _table.ExecuteAsync(operation);
            if (result.Result is EngineEntity entity)
            {
                return new EngineDto
                {
                    Id = id,
                    Name = entity.Name,
                    Manufacturer = entity.Manufacturer,
                    Value = entity.Value,
                    PictureUrl = entity.PictureUrl,
                    WeeklyWearOff = entity.WeeklyWearDown,
                    MaximumWearOff = entity.MaximumWearDown
                };
            }

            throw new TeamComponentNotFoundException(TeamComponent.Engine, id);
        }

        public EnginesReadRespository(IOptions<TeamsOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageConnectionString;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }

    }
}