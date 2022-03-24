using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.ActualTeams.DataTransferObjects;
using F1Manager.Admin.ActualTeams.DomainModel;

namespace F1Manager.Admin.ActualTeams.Abstractions;

public interface IActualTeamsRepository
{
    Task<List<ActualTeamDetailsDto>> GetActive();
    Task<List<ActualTeamDetailsDto>> GetList(ActualTeamListFilterDto filter);
    Task<ActualTeam> Get(Guid id);
    Task<ActualTeamDetailsDto> GetById(Guid id);
    Task<bool> Create(ActualTeam domainModel);
    Task<bool> Update(ActualTeam domainModel);
}