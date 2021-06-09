using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
   public static  class DriverMappings
    {

        public static Driver ToDomainModel(this DriverEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Driver(entity.Id,
                entity.Name,
                entity.DateOfBirth,
                entity.CountryOfOrigin,
                entity.CurrentValue,
                entity.PictureUrl,
                entity.ActiveFrom,
                entity.ActiveUntil,
                entity.IsAvailable,
                entity.IsDeleted);
        }

        public static DriverEntity ToEntity(this Driver model, DriverEntity existing = null)
        {
            var entity = existing ?? new DriverEntity
            {
                Id = model.Id
            };
            entity.Name = model.Name;
            entity.DateOfBirth = model.DateOfBirth;
            entity.CountryOfOrigin = model.CountryOfOrigin;
            entity.CurrentValue = model.CurrentValue;
            entity.ActiveFrom= model.ActiveFrom;
            entity.ActiveUntil = model.ActiveUntil;
            entity.PictureUrl = model.PictureUrl;
            entity.IsAvailable= model.IsAvailable;
            entity.IsDeleted = model.IsDeleted;
            return entity;
        }

    }
}
