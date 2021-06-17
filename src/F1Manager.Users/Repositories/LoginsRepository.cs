using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.DataTransferObjects;
using F1Manager.Users.Domain;
using F1Manager.Users.Entities;
using F1Manager.Users.Exceptions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Users.Repositories
{
    public sealed class LoginsRepository: ILoginsRepository
    {
        public const string TableName = "Logins";
        private const string PartitionKey = "attempts";
        private CloudTable _table;

        public async Task<bool> RegisterLoginAttempt(LoginAttempt attempt)
        {
            var entity = new LoginAttemptEntity
            {
                PartitionKey = PartitionKey,
                RowKey = attempt.Id.ToString(),
                RsaPrivateKey = attempt.RsaPrivateKey,
                IssuedOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15)
            };
            var op = TableOperation.Insert(entity);
            var result = await _table.ExecuteAsync(op);
            return result.HttpStatusCode.IsSuccess();
        }

        public async Task<LoginAttempt> ValidateLoginAttempt(Guid loginAttempt)
        {
            var operation = TableOperation.Retrieve<LoginAttemptEntity>(PartitionKey, loginAttempt.ToString());
            var result = await _table.ExecuteAsync(operation);
            if (result.HttpStatusCode.IsSuccess())
            {
                var entity = result.Result as LoginAttemptEntity;
                return new LoginAttempt(Guid.Parse(entity.RowKey),
                    entity.RsaPrivateKey);
            }
            throw new F1ManagerLoginException(LoginErrorCode.LoginAttemptFailedOrExpired,
                "The login attempt could not be validated. It either does not exist, or is expired.");

        }

        public static async Task Cleanup(CloudTable cloudTable)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(LoginAttemptEntity.PartitionKey),
                QueryComparisons.Equal, PartitionKey);
            var dateFilter = TableQuery.GenerateFilterConditionForDate(nameof(LoginAttemptEntity.ExpiresOn),
                QueryComparisons.LessThanOrEqual, DateTimeOffset.UtcNow);

            var allFilters = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, dateFilter);

            var query = new TableQuery<LoginAttemptEntity>().Where(allFilters);


            var expiredLoginAttempts = new List<LoginAttemptEntity>();
            TableContinuationToken ct = null;
            do
            {
                var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, ct);
                expiredLoginAttempts.AddRange(segment.Results);
            } while (ct != null);


            var batch = new TableBatchOperation();
            foreach (var expiredLoginAttempt in expiredLoginAttempts)
            {
                batch.Add(TableOperation.Delete(expiredLoginAttempt));
                if (batch.Count >= 90)
                {
                    await cloudTable.ExecuteBatchAsync(batch);
                }
            }

            if (batch.Count > 0)
            {
                await cloudTable.ExecuteBatchAsync(batch);
            }
        }

        public LoginsRepository(IOptions<UsersOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }

    }
}
