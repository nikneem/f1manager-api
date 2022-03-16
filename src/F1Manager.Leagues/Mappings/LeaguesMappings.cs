using System;
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
                PartitionKey = LeaguesRepository.LeaguesPartitionKey,
                RowKey = domainModel.Id.ToString(),
                OwnerId = domainModel.OwnerId,
                SeasonId = domainModel.SeasonId,
                Name = domainModel.Name,
                MembersCount = domainModel.Members.Count,
                CreatedOn = domainModel.CreatedOn,
                ETag = "*"
            };
        }
        public static LeagueMemberEntity ToEntity(this LeagueMember domainModel, Guid leagueId)
        {
            return new LeagueMemberEntity
            {
                PartitionKey = leagueId.ToString(),
                RowKey = domainModel.TeamId.ToString(),
                IsMaintainer = domainModel.IsMaintainer,
                JoinedOn = domainModel.CreatedOn,
                ETag = "*"
            };
        }
        public static LeagueInvitationEntity ToEntity(this LeagueInvitation domainModel)
        {
            return new LeagueInvitationEntity
            {
                PartitionKey = domainModel.LeagueId.ToString(),
                RowKey = domainModel.TeamId.ToString(),
                AcceptedOn = domainModel.AcceptedOn,
                DeclinedOn = domainModel.DeclinedOn,
                CreatedOn = domainModel.CreatedOn,
                ExpiresOn = domainModel.ExpiresOn,
                ETag = "*"
            };
        }
        public static LeagueRequestEntity ToEntity(this LeagueRequest domainModel)
        {
            return new LeagueRequestEntity
            {
                PartitionKey = domainModel.LeagueId.ToString(),
                RowKey = domainModel.TeamId.ToString(),
                AcceptedOn = domainModel.AcceptedOn,
                DeclinedOn = domainModel.DeclinedOn,
                CreatedOn = domainModel.CreatedOn,
                ExpiresOn = domainModel.ExpiresOn,
                ETag = "*"
            };
        }
    }
}
