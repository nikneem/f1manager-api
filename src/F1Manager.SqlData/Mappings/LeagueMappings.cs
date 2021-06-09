using System;
using System.Collections.Generic;
using System.Linq;
using F1Manager.SqlData.Entities;

namespace F1Manager.SqlData.Mappings
{
    public static class LeagueMappings
    {

        public static LeagueEntity ToEntity(this League model, LeagueEntity existing = null)
        {
            var entity = existing ?? new LeagueEntity
            {
                Id = model.Id,
                CreatedOn = model.CreatedOn,
                PlayerId = model.OwnerPlayerId,
                SeasonId = model.SeasonId
            };
            entity.Name = model.Name;

            

            return entity;
        }
        public static League ToDomainModel(this LeagueEntity entity)
        {
            var members = GetMembers(entity);
            var invitations = GetInvitations(entity);
            var requests = GetRequests(entity);
            return new League
            (
                entity.Id,
                entity.SeasonId,
                entity.PlayerId,
                entity.Name,
                entity.CreatedOn,
                members,
                invitations,
                requests
            );
        }

        public static LeagueMemberEntity ToEntity(this LeagueMember model, Guid leagueId, LeagueMemberEntity existing = null)
        {
            var entity = existing ?? new LeagueMemberEntity
            {
                Id = model.Id,
                LeagueId = leagueId,
                TeamId =  model.TeamId,
                AllowRemoval =  model.AllowRemoval
            };
            entity.AllowRemoval = model.AllowRemoval;
            return entity;
        }
        public static LeagueInviteEntity ToEntity(this LeagueInvite model, Guid leagueId, LeagueInviteEntity existing = null)
        {
            var entity = existing ?? new LeagueInviteEntity
            {
                Id = model.Id,
                LeagueId = leagueId,
                TeamId =  model.TeamId,
                InvitedOn = model.InvitedOn,
            };
            entity.AcceptedOn = model.AcceptedOn;
            entity.DeniedOn = model.DeniedOn;

            return entity;
        }
        public static LeagueRequestEntity ToEntity(this LeagueRequest model, Guid leagueId, LeagueRequestEntity existing = null)
        {
            var entity = existing ?? new LeagueRequestEntity
            {
                Id = model.Id,
                LeagueId = leagueId,
                TeamId =  model.TeamId,
                RequestedOn = model.RequestedOn,
            };
            entity.AcceptedOn = model.AcceptedOn;
            entity.DeniedOn = model.DeniedOn;

            return entity;
        }


        private static List<LeagueMember> GetMembers(LeagueEntity entity)
        {
            return entity.Members?.Select(m => new LeagueMember(
                    m.Id,
                    m.TeamId,
                    m.AllowRemoval))
                .ToList();
        }
        private static List<LeagueInvite> GetInvitations(LeagueEntity entity)
        {
            return entity.Invites?.Select(m => new LeagueInvite(
                    m.Id,
                    m.TeamId,
                    m.InvitedOn,
                    m.AcceptedOn,
                    m.DeniedOn))
                .ToList();
        }
        private static List<LeagueRequest> GetRequests(LeagueEntity entity)
        {
            return entity.Requests?.Select(m => new LeagueRequest(
                    m.Id,
                    m.TeamId,
                    m.RequestedOn,
                    m.AcceptedOn,
                    m.DeniedOn))
                .ToList();
        }

    }
}
