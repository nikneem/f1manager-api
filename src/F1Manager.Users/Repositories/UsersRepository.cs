using System;
using System.Threading.Tasks;
using F1Manager.Shared.Enums;
using F1Manager.Shared.Helpers;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.Domain;
using F1Manager.Users.Entities;
using F1Manager.Users.Mappings;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace F1Manager.Users.Repositories
{
    public sealed class UsersRepository: IUsersRepository
    {

        private const string UsersTableName = "Users";
        public const string PartitionKey = "user";
        private CloudTable _table;

        public async Task<bool> GetIsUsernameUnique(Guid id, string username)
        {
            var loweredUsername = username.ToLower();
            var retrieveOperation = TableOperation.Retrieve<UserEntity>(PartitionKey, loweredUsername);
            var result = await _table.ExecuteAsync(retrieveOperation);
            if (result.Result is UserEntity entity)
            {
                return Equals(id, entity.SubjectId);
            }

            return true;
        }

        public async Task<bool> Insert(User userDomainModel)
        {
            await _table.CreateIfNotExistsAsync();
            if (userDomainModel.TrackingState == TrackingState.New)
            {
                var userEntity = userDomainModel.ToEntity();
                var insertOperation = TableOperation.Insert(userEntity);
                var result = await _table.ExecuteAsync(insertOperation);
                return result.HttpStatusCode == 204;
            }

            return false;
        }

            public UsersRepository(IOptions<UsersOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageConnectionString;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(UsersTableName);
        }
    }
}
