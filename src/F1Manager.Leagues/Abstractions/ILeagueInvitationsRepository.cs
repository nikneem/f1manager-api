using System;
using System.Threading.Tasks;
using F1Manager.Leagues.DomainModels;

namespace F1Manager.Leagues.Abstractions;

public interface ILeagueInvitationsRepository
{
    Task<LeagueInvitation> Get(Guid leagueId, Guid teamId);
    Task<bool> Create(LeagueInvitation domainModel);
    Task<bool> Update(LeagueInvitation domainModel);
}