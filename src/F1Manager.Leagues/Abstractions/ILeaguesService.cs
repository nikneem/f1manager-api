using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Leagues.DataTransferObjects;
using F1Manager.Shared.DataTransferObjects;

namespace F1Manager.Leagues.Abstractions;

public interface ILeaguesService
{
    Task<CollectionResult<LeagueListDto>> List(LeaguesListFilterDto filter);
    Task<List<LeagueListDto>> List(Guid teamId);
    Task<LeagueDto> Get(Guid leagueId, Guid teamId, Guid userId);
    Task<LeagueDto> Create(CreateLeagueDto dto, Guid userId, Guid teamId);
    Task<LeagueDto> Update(LeagueDto dto, Guid userId, Guid teamId);
    Task<bool> Invite(Guid leagueId, Guid userId, Guid teamId, Guid inviteTeamId);
    Task<bool> AcceptInvitation(Guid leagueId, Guid teamId);
    Task<bool> DeclineInvitation(Guid leagueId, Guid teamId);
    Task<List<LeagueRequestDto>> ListRequests(Guid leagueId, Guid userId, Guid teamId);
    Task<bool> Request(Guid leagueId, Guid userId, Guid teamId);
    Task<bool> AcceptRequest(Guid leagueId, Guid userId, Guid teamId, Guid requestTeamId);
    Task<bool> DeclineRequest(Guid leagueId, Guid userId, Guid teamId, Guid requestTeamId);
    Task<bool> Validate(CreateLeagueDto dto);
}