using F1Manager.Users.Domain;
using F1Manager.Users.Entities;
using F1Manager.Users.Repositories;

namespace F1Manager.Users.Mappings;

public static class UserMappings
{

    public static UserEntity ToEntity(this User domainModel, UserEntity? existingEntity = null)
    {
        var entity = existingEntity ?? new UserEntity
        {
            PartitionKey = UsersRepository.PartitionKey,
            RowKey = domainModel.Username,
            SubjectId = domainModel.Id,
            RegisteredOn = domainModel.RegisteredOn,
        };

        entity.DisplayName = domainModel.DisplayName;
        entity.Password = domainModel.Password.EncryptedPassword;
        entity.Salt = domainModel.Password.Salt;
        entity.DateEmailVerified = domainModel.EmailAddress.VerificationOn;
        entity.DueDateEmailVerified = domainModel.EmailAddress.VerificationDueOn;
        entity.EmailAddress = domainModel.EmailAddress.Value;
        entity.LockoutReason = domainModel.LockoutReason;
        entity.LastLoginOn = domainModel.LastLoginOn;
        entity.EmailVerificationCode = domainModel.EmailAddress.VerificationCode;
        entity.NewPassword = domainModel.NewPassword?.EncryptedPassword;
        entity.NewPasswordSalt = domainModel.NewPassword?.Salt;
        entity.NewPasswordVerification = domainModel.NewPasswordVerification;
        entity.IsAdministrator = domainModel.IsAdministrator;
        entity.ETag = "*";
        return entity;
    }

}