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
    public sealed class DriversReadRepository : IDriversReadRepository
    {

        private const string TableName = "Components";
        public const string PartitionKey = "driver";

        private readonly CloudTable _table;

        public async Task<DriverDto> GetById(Guid id)
        {
            var operation = TableOperation.Retrieve<DriverEntity>(PartitionKey, id.ToString());
            var result = await _table.ExecuteAsync(operation);
            if (result.Result is DriverEntity entity)
            {
                return new DriverDto
                {
                    Id = id,
                    Name = entity.Name,
                    Country = entity.Country,
                    DateOfBirth = entity.DateOfBirth,
                    Value = entity.Value,
                    PictureUrl = entity.PictureUrl
                };
            }

            throw new TeamComponentNotFoundException(TeamComponent.FirstDriver, id);
        }

        public DriversReadRepository(IOptions<TeamsOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageConnectionString;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }

    }
}