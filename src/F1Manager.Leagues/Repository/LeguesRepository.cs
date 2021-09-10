using System.Threading.Tasks;
using F1Manager.Leagues.Configuration;
using F1Manager.Leagues.DomainModels;
using F1Manager.Leagues.Mappings;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Leagues.Repository
{
    public sealed class LeaguesRepository
    {

        private const string TableName = "Leagues";
        public const string PartitionKey = "league";
        private CloudTable _table;

        public async Task<bool> Insert(League domainModel)
        {
            if (domainModel.TrackingState == TrackingState.New)
            {
                var userEntity = domainModel.ToEntity();
                var insertOperation = TableOperation.Insert(userEntity);
                var result = await _table.ExecuteAsync(insertOperation);
                return result.HttpStatusCode.IsSuccess();
            }

            return false;
        }
        public async Task<bool> Update(League domainModel)
        {
            if (domainModel.TrackingState == TrackingState.Modified)
            {
                var userEntity = domainModel.ToEntity();
                var insertOperation = TableOperation.Replace(userEntity);
                var result = await _table.ExecuteAsync(insertOperation);
                return result.HttpStatusCode.IsSuccess();
            }

            return false;
        }



        public LeaguesRepository(IOptions<LeaguesOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }

    }
}
