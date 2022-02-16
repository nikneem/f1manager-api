using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Leagues.DataTransferObjects;

namespace F1Manager.Leagues.Abstractions;

public interface ILeaguesService
{
    Task<List<LeagueListDto>> List(Guid teamId);
    Task<LeagueDto> Get(Guid leagueId, Guid teamId);
    Task<LeagueDto> Create(CreateLeagueDto dto, Guid userId, Guid teamId);
    Task<LeagueDto> Update(LeagueDto dto, Guid userId, Guid teamId);
    Task<bool> Invite(Guid leagueId, Guid userId, Guid teamId, Guid inviteTeamId);
    Task<bool> AcceptInvitation(Guid leagueId, Guid teamId);
    Task<bool> DeclineInvitation(Guid leagueId, Guid teamId);
}