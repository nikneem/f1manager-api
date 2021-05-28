using F1Manager.Users.Domain;
using F1Manager.Users.Entities;
using F1Manager.Users.Repositories;

namespace F1Manager.Users.Mappings
{
    public static class UserMappings
    {

        public static UserEntity ToEntity(this User domainModel)
        {
            return new UserEntity
            {
                PartitionKey = UsersRepository.PartitionKey,
                RowKey = domainModel.Username,
                SubjectId = domainModel.Id,
                DisplayName = domainModel.DisplayName,
                Password = domainModel.Password.EncryptedPassword,
                DateEmailVerified = domainModel.DateEmailVerified,
                DueDateEmailVerified = domainModel.DueDateEmailVerified,
                EmailAddress = domainModel.EmailAddress,
                LockoutReason = domainModel.LockoutReason,
                RegisteredOn = domainModel.RegisteredOn,
                LastLoginOn = domainModel.LastLoginOn,
                EmailVerificationCode = domainModel.EmailVerificationCode,
                IsAdministrator = domainModel.IsAdministrator
            };
        }

    }
}
