using System.Collections.Generic;
using System.Linq;
using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class UserMappings
    {
        public static UserEntity ToEntity(this User model, UserEntity existing = null)
        {
            var entity = existing ?? new UserEntity {Id = model.Id};
            entity.Username = model.Username;
            entity.EmailAddress = model.EmailAddress;
            entity.DisplayName = model.DisplayName;
            return entity;
        }

        public static UserCredentialEntity ToEntity(this Credential model, UserCredentialEntity existing = null)
        {
            var entity = existing ?? new UserCredentialEntity {Id = model.Id};
            entity.SubjectId = model.SubjectId;
            entity.IsFromExternalProvider = model.IsFromExternalProvider;
            return entity;
        }

        public static User ToDomainModel(this UserEntity entity)
        {
            if (entity != null)
            {
                var credentials = entity.Credentials?.ToDomainModel();
                return new User(entity.Id, entity.Username, entity.DisplayName, entity.EmailAddress, credentials);
            }

            return null;
        }

        public static Credential ToDomainModel(this UserCredentialEntity entity)
        {
            if (entity != null)
            {
                return new Credential(entity.Id, entity.SubjectId, entity.IsFromExternalProvider);
            }

            return null;
        }

        public static List<Credential> ToDomainModel(this List<UserCredentialEntity> entites)
        {
            return entites?.Select(x => x.ToDomainModel()).ToList();
        }

    }
}