using System;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Shared.Enums;
using F1Manager.Shared.Helpers;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.Domain;
using F1Manager.Users.Entities;
using F1Manager.Users.Mappings;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Users.Repositories
{
    public sealed class UsersRepository : IUsersRepository
    {

        private const string TableName = "Users";
        public const string PartitionKey = "user";
        private readonly CloudTable _table;

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

        public async Task<User> GetByUsername(string username)
        {
            var loweredUsername = username.ToLower();
            var retrieveOperation = TableOperation.Retrieve<UserEntity>(PartitionKey, loweredUsername);
            var result = await _table.ExecuteAsync(retrieveOperation);
            if (result.Result is UserEntity entity)
            {
                return ToDomainModel(entity);
            }

            return null;
        }

        public async Task<bool> Insert(User userDomainModel)
        {
            if (userDomainModel.TrackingState == TrackingState.New)
            {
                var userEntity = userDomainModel.ToEntity();
                var insertOperation = TableOperation.Insert(userEntity);
                var result = await _table.ExecuteAsync(insertOperation);
                return result.HttpStatusCode == 204;
            }

            return false;
        }

        public async Task<User> GetById(Guid userId)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(UserEntity.PartitionKey),
                QueryComparisons.Equal, PartitionKey);
            var userSubjectFilter = TableQuery.GenerateFilterConditionForGuid(nameof(UserEntity.SubjectId),
                QueryComparisons.Equal, userId);
            var filterString = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, userSubjectFilter);

            TableQuery<UserEntity> query = new TableQuery<UserEntity>().Where(filterString).Take(1);
            var segment = await _table.ExecuteQuerySegmentedAsync(query, null);
            var userEntity = segment.Results.FirstOrDefault();
            return ToDomainModel(userEntity);

        }

        private static User ToDomainModel(UserEntity entity)
        {
            return new User(entity.SubjectId,
                entity.DisplayName,
                entity.RowKey,
                entity.Password,
                entity.Salt,
                entity.EmailAddress,
                entity.DateEmailVerified,
                entity.DueDateEmailVerified,
                entity.LockoutReason,
                entity.IsAdministrator,
                entity.RegisteredOn,
                entity.LastLoginOn);
        }

        public UsersRepository(IOptions<UsersOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }
    }
}
