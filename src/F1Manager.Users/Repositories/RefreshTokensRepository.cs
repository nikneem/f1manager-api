using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.Entities;
using F1Manager.Users.Exceptions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Users.Repositories
{
    public sealed class RefreshTokensRepository : IRefreshTokensRepository
    {

        public const string TableName = "RefreshTokens";
        private const string PartitionKey = "tokens";
        private CloudTable _table;


        public async Task<Guid> ValidateRefreshToken(string token)
        {
            var operation = TableOperation.Retrieve<RefreshTokenEntity>(PartitionKey, token);
            var result = await _table.ExecuteAsync(operation);
            if (result.HttpStatusCode.IsSuccess())
            {
                if (result.Result is RefreshTokenEntity entity)
                {
                    if (!entity.IsActive)
                    {
                        throw new F1ManagerLoginException(LoginErrorCode.InactiveRefreshToken,
                            "The token is inactive and cannot be used anymore");
                    }

                    if (entity.IsRevoked)
                    {
                        throw new F1ManagerLoginException(LoginErrorCode.RevokedRefreshToken,
                            "This refresh token is revoked and now handled as compromised");
                    }

                    if (entity.ExpiresOn <= DateTimeOffset.UtcNow)
                    {
                        throw new F1ManagerLoginException(LoginErrorCode.ExpiredRefreshToken,
                            "The refresh token is expired. User needs to log on manually");
                    }

                    entity.IsActive = false;
                    entity.ExpiresOn = DateTimeOffset.UtcNow;
                    var updateOperation = TableOperation.Replace(entity);
                    var updateResult = await _table.ExecuteAsync(updateOperation);
                    if (updateResult.HttpStatusCode.IsSuccess())
                    {
                        return entity.UserId;
                    }
                }
            }

            throw new F1ManagerLoginException(LoginErrorCode.InvalidRefreshToken,
                "A refresh token was attempted to be used but the token is invalid and was not found");
        }
        public async Task<string> GenerateRefreshToken(Guid userId, string ipAddress)
        {
            var tokenString = Randomize.GenerateRefreshToken();
            var token = new RefreshTokenEntity
            {
                PartitionKey = PartitionKey,
                RowKey = tokenString,
                UserId = userId,
                ExpiresOn = DateTimeOffset.UtcNow.AddDays(14),
                GeneratedOn = DateTimeOffset.UtcNow,
                IpAddress = ipAddress,
                IsRevoked = false,
                IsActive = true
            };

            var operation = TableOperation.Insert(token);
            var result = await _table.ExecuteAsync(operation);
            if (result.HttpStatusCode.IsSuccess())
            {
                return tokenString;
            }

            throw new F1ManagerLoginException(LoginErrorCode.InvalidRefreshTokenOperation,
                "Operation failed, could not create a valid refresh token");
        }
        public async Task RevokeAll(Guid userId)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(RefreshTokenEntity.PartitionKey),
                QueryComparisons.Equal, PartitionKey);
            var userIdFilter = TableQuery.GenerateFilterConditionForGuid(nameof(RefreshTokenEntity.UserId),
                QueryComparisons.Equal, userId);
            var filterString = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, userIdFilter);

            TableQuery<RefreshTokenEntity> query = new TableQuery<RefreshTokenEntity>().Where(filterString);

            var userRefreshTokens = new List<RefreshTokenEntity>();
            TableContinuationToken ct = null;
            do
            {
                var segment = await _table.ExecuteQuerySegmentedAsync(query, ct);
                userRefreshTokens.AddRange(segment.Results);
                ct = segment.ContinuationToken;
            } while (ct != null);

            var batch = new TableBatchOperation();
            foreach (var refreshTokenEntity in userRefreshTokens)
            {
                refreshTokenEntity.IsRevoked = true;
                batch.Add(TableOperation.Replace(refreshTokenEntity));
                if (batch.Count >= 90)
                {
                    await ExecuteBatchAndValidateResponse(batch);
                }
            }
            if (batch.Count >= 0)
            {
                await ExecuteBatchAndValidateResponse(batch);
            }

        }
        private async Task ExecuteBatchAndValidateResponse(TableBatchOperation batch)
        {
            var response = await _table.ExecuteBatchAsync(batch);
            if (!response.TrueForAll(x => x.HttpStatusCode.IsSuccess()))
            {
                throw new F1ManagerLoginException(LoginErrorCode.InvalidRefreshTokenOperation,
                    "Failed to revoke one or more refresh tokens");
            }
        }

        public static async Task Cleanup(CloudTable cloudTable)
        {
            var partitionKeyFilter = TableQuery.GenerateFilterCondition(nameof(RefreshTokenEntity.PartitionKey),
                QueryComparisons.Equal, PartitionKey);
            var revokedFilter = TableQuery.GenerateFilterConditionForBool(nameof(RefreshTokenEntity.IsRevoked),
                QueryComparisons.Equal, true);
            var activeFilter = TableQuery.GenerateFilterConditionForBool(nameof(RefreshTokenEntity.IsActive),
                QueryComparisons.Equal, false);
            var dateFilter = TableQuery.GenerateFilterConditionForDate(nameof(RefreshTokenEntity.ExpiresOn),
                QueryComparisons.LessThanOrEqual, DateTimeOffset.UtcNow);

            var rovekedAndActive = TableQuery.CombineFilters(revokedFilter, TableOperators.Or, activeFilter);
            var allFilters = TableQuery.CombineFilters(dateFilter, TableOperators.Or, rovekedAndActive);
            var finalFilter = TableQuery.CombineFilters(partitionKeyFilter, TableOperators.And, allFilters);

            var query = new TableQuery<RefreshTokenEntity>().Where(finalFilter);


            var oldRefreshTokens = new List<RefreshTokenEntity>();
            TableContinuationToken ct = null;
            do
            {
                var segment = await cloudTable.ExecuteQuerySegmentedAsync(query, ct);
                oldRefreshTokens.AddRange(segment.Results);
                ct = segment.ContinuationToken;
            } while (ct != null);


            var batch = new TableBatchOperation();
            foreach (var refreshTokenEntity in oldRefreshTokens)
            {
                batch.Add(TableOperation.Delete(refreshTokenEntity));
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

        public RefreshTokensRepository(IOptions<UsersOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(TableName);
        }

    }
}
