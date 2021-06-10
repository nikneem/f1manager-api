using System;
using System.Threading.Tasks;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.DataTransferObjects;
using F1Manager.Users.Entities;
using F1Manager.Users.Exceptions;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Options;

namespace F1Manager.Users.Repositories
{
    public sealed class LoginRepository: ILoginRepository
    {
        private const string LoginsTable = "Logins";
        private const string PartitionKey = "attempts";
        private CloudTable _table;

        public async Task<LoginAttemptDto> RegisterLoginAttempt(string key, string iv)
        {
            var attemptId = Guid.NewGuid();
            var entity = new LoginAttemptEntity
            {
                PartitionKey = PartitionKey,
                RowKey = attemptId.ToString(),
                SecurityKey = key,
                SecurityVector = iv,
                IssuedOn = DateTimeOffset.UtcNow,
                ExpiresOn = DateTimeOffset.UtcNow.AddMinutes(15)
            };
            var op = TableOperation.Insert(entity);
            var result = await _table.ExecuteAsync(op);
            if (result.HttpStatusCode.IsSuccess())
            {
                return new LoginAttemptDto
                {
                    Id = attemptId,
                    Key = entity.SecurityKey,
                    Vector = entity.SecurityVector
                };
            }

            throw new F1ManagerLoginException(LoginErrorCode.FailedToRegisterLoginAttempt,
                "Failed to persist new login attempt. Login cannot take place");
        }

        public async Task<LoginAttemptDto> ValidateLoginAttempt(Guid loginAttempt)
        {
            var operation = TableOperation.Retrieve<LoginAttemptEntity>(PartitionKey, loginAttempt.ToString());
            var result = await _table.ExecuteAsync(operation);
            if (result.HttpStatusCode.IsSuccess())
            {
                var entity = result.Result as LoginAttemptEntity;
                return new LoginAttemptDto
                {
                    Id = Guid.Parse(entity.RowKey),
                    Key = entity.SecurityKey,
                    Vector = entity.SecurityVector
                };
            }
            throw new F1ManagerLoginException(LoginErrorCode.LoginAttemptFailedOrExpired,
                "The login attempt could not be validated. It either does not exist, or is expired.");

        }


        public LoginRepository(IOptions<UsersOptions> config)
        {
            var storageAccountConnectionString = config.Value.AzureStorageAccount;
            var storageAccount = TableStorageHelper.CreateStorageAccountFromConnectionString(storageAccountConnectionString);
            var tableClient = storageAccount.CreateCloudTableClient(new TableClientConfiguration());
            _table = tableClient.GetTableReference(LoginsTable);
        }

    }
}
