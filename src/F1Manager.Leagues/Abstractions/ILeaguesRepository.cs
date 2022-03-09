using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Leagues.DataTransferObjects;
using F1Manager.Leagues.DomainModels;

namespace F1Manager.Leagues.Abstractions;

public interface ILeaguesRepository
{
    Task<List<LeagueListDto>> List();
    Task<List<LeagueListDto>> List(Guid teamId);
    Task<League> Get(Guid leagueId);
    Task<bool> Create(League domainModel);
    Task<bool> Update(League domainModel);
    Task<bool> IsUniqueName(string name, int season);
}