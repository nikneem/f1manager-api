using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class ChassisMappings
    {
        public static Chassis ToDomainModel(this ChassisEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Chassis(
                entity.Id,
                entity.Name,
                entity.PictureUrl,
                entity.CurrentValue,
                entity.WeeklyWearOutPercentage,
                entity.MaxWearOutPercentage,
                entity.ActiveFrom,
                entity.ActiveUntil,
                entity.IsAvailable,
                entity.IsDeleted);
        }

        public static ChassisEntity ToEntity(this Chassis model, ChassisEntity existing = null)
        {
            var entity = existing ?? new ChassisEntity
            {
                Id = model.Id
            };
            entity.Name = model.Name;
            entity.PictureUrl = model.PictureUrl;
            entity.CurrentValue = model.CurrentValue;
            entity.WeeklyWearOutPercentage = model.WeeklyWearOutPercentage;
            entity.MaxWearOutPercentage = model.MaxWearOutPercentage;
            entity.ActiveFrom = model.ActiveFrom;
            entity.ActiveUntil = model.ActiveUntil;
            entity.IsAvailable = model.IsAvailable;
            entity.IsDeleted = model.IsDeleted;

            return entity;
        }
    }
}