using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class EngineMappings
    {

        public static Engine ToDomainModel(this EngineEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Engine(entity.Id,
                entity.Name,
                entity.Manufacturer,
                entity.Model,
                entity.PictureUrl,
                entity.CurrentValue,
                entity.WeeklyWearOutPercentage,
                entity.MaxWearOutPercentage,
                entity.ActiveFrom,
                entity.ActiveUntil,
                entity.IsAvailable);
        }

    }
}
