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


    public sealed class ChassisReadRespository : IChassisReadRespository
    {
        private const string TableName = "Components";
        public const string PartitionKey = "chassis";

        private readonly CloudTable _table;

        public async Task<ChassisDto> GetById(Guid id)
        {
            var operation = TableOperation.Retrieve<ChassisEntity>(PartitionKey, id.ToString());
            var result = await _table.ExecuteAsync(operation);
            if (result.Result is ChassisEntity entity)
            {
                return new ChassisDto
                {
                    Id = id,
                    Name = entity.Name,
                    Manufacturer = entity.Manufacturer,
                    Value = Convert.ToDecimal(entity.Value),
                    PictureUrl = entity.PictureUrl,
                    WeeklyWearOff = Convert.ToDecimal(entity.WeeklyWearDown),
                    MaximumWearOff = Convert.ToDecimal(entity.MaximumWearDown)
                };
            }

            throw new TeamComponentNotFoundException(TeamComponent.Chassis, id);
        }

        public ChassisReadRespository(IOptions<TeamsOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }
    }
}
