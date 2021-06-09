using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
   public static  class TeamMappings
    {

        public static TeamEntity ToEntity(this Team domainModel, TeamEntity existingEntity)
        {
            var entity = existingEntity ?? new TeamEntity {Id = domainModel.Id};
            entity.Name = domainModel.Name;
            entity.IsPublic = domainModel.IsPublic;
            entity.Money = domainModel.Money;
            entity.Points = domainModel.Points;
            return entity;
        }

    }
}
