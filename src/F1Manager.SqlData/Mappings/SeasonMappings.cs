using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class SeasonMappings
    {
        public static Season ToDomainModel(this SeasonEntity entity)
        {
            if (entity == null)
            {
                return null;
            }

            return new Season(entity.Id, entity.Name, entity.ActiveFrom, entity.ActiveUntil);
        }
    }
}