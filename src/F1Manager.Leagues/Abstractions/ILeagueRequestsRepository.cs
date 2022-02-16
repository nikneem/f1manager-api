using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Leagues.DomainModels;

namespace F1Manager.Leagues.Abstractions;

public interface ILeagueRequestsRepository
{
    Task<List<LeagueInvitation>> Get(Guid leagueId);
    Task<LeagueInvitation> Get(Guid leagueId, Guid teamId);
    Task<bool> Create(LeagueRequest domainModel);
    Task<bool> Update(LeagueRequest domainModel);
}