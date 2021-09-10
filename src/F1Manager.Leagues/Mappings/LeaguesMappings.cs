using System.Runtime.InteropServices;
using F1Manager.Leagues.DomainModels;
using F1Manager.Leagues.Entities;
using F1Manager.Leagues.Repository;

namespace F1Manager.Leagues.Mappings
{
    public static class LeaguesMappings
    {

        public static LeagueEntity ToEntity(this League domainModel)
        {
            return new LeagueEntity
            {
                PartitionKey = LeaguesRepository.PartitionKey,
                RowKey = domainModel.Id.ToString(),
                OwnerId = domainModel.OwnerId,
                SeasonId = domainModel.SeasonId,
                Name = domainModel.Name,
                CreatedOn = domainModel.CreatedOn,
                ETag = "*"
            };
        }
    }
}
