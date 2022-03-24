using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.ActualTeams.DataTransferObjects;
using F1Manager.Admin.Engines.DataTransferObjects;

namespace F1Manager.Admin.ActualTeams.Abstractions;

public interface IActualTeamsService
{
    Task<List<ActualTeamDetailsDto>> GetActive();
    Task<List<ActualTeamDetailsDto>> List(ActualTeamListFilterDto filter);
    Task<ActualTeamDetailsDto> Get(Guid id);
    Task<ActualTeamDetailsDto> Create(ActualTeamDetailsDto dto);
    Task<ActualTeamDetailsDto> Update(Guid id, ActualTeamDetailsDto dto);
    Task<bool> Delete(Guid id);
    Task<bool> Undelete(Guid id);
}