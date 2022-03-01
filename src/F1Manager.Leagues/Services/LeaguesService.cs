using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Leagues.Abstractions;
using F1Manager.Leagues.Configuration;
using F1Manager.Leagues.DataTransferObjects;
using F1Manager.Leagues.DomainModels;
using F1Manager.Leagues.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace F1Manager.Leagues.Services;

public class LeaguesService : CachedServiceBase<LeaguesService>, ILeaguesService
{
    private readonly ILeaguesRepository _repository;
    private readonly ILeagueInvitationsRepository _invitationsRepository;
    private readonly ILeaguesDomainService _domainService;
    private readonly ILogger<LeaguesService> _logger;

    public  Task<List<LeagueListDto>> List(Guid teamId)
    {
        var cachekey = $"{CacheKeyPrefixes.LeaguesByTeam}{teamId}";
        return GetFromCache(cachekey, () => GetLeaguesByMembership(teamId));
    }
    public async Task<LeagueDto> Get(Guid leagueId, Guid teamId, Guid userId)
    {
        var cachekey = $"{CacheKeyPrefixes.LeagueDetails}{leagueId}";
        var leagueDetails = await GetFromCache(cachekey, () => GetLeagueDetails(leagueId, teamId, userId));
        if (leagueDetails.Members.Any(x => x.TeamId == teamId))
        {
            return leagueDetails;
        }

        return null;
    }
    public async Task<LeagueDto> Create(CreateLeagueDto dto, Guid userId, Guid teamId)
    {
        await InvalidateLeaguesListCache(teamId);
        _logger.LogInformation("About to create a new league for user {userId} and team {teamId}", userId, teamId);
        var league = await League.Create(userId, dto.Name, _domainService);
        league.AddMember(teamId);
        if (await _repository.Create(league))
        {
            return ToDto(league,teamId, userId);
        }

        return null;
    }
    public async Task<LeagueDto> Update(LeagueDto dto, Guid userId, Guid teamId)
    {
        await InvalidateLeaguesListCache(teamId);
        await InvalidateLeaguesDetailsCache(dto.Id);
        var league = await _repository.Get(dto.Id);
        if (IsOwnerOrMaintainer(userId, teamId, league))
        {
            await league.SetName(dto.Name, _domainService);
            if (await _repository.Update(league))
            {
                return ToDto(league, teamId, userId);
            }
        }

        return null;
    }

    public async Task<bool> Invite(Guid leagueId, Guid userId, Guid teamId, Guid inviteTeamId)
    {
        var league = await _repository.Get(leagueId);
        if (IsOwnerOrMaintainer(userId, teamId, league))
        {
            if (league.Members.Any(m => m.TeamId == inviteTeamId))
            {
                throw new F1ManagerLeaguesException(LeagueErrorCode.AlreadyMemberOfLeague,
                    "This team is already member of this league");
            }

            var invitation = new LeagueInvitation(leagueId, inviteTeamId);
            try
            {
                return await _invitationsRepository.Create(invitation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to create an invitation for league {leagueId} and team {teamId}", leagueId, teamId);
                return false;
            }
        }

        return false;
    }
    public async Task<bool> AcceptInvitation(Guid leagueId, Guid teamId)
    {
        bool updatedSuccessfully = false;
        var invitation = await _invitationsRepository.Get(leagueId, teamId);
        var league = await _repository.Get(leagueId);
        if (invitation != null)
        {
            invitation.Accept();
            league.AddMember(invitation.TeamId);
            updatedSuccessfully = await _invitationsRepository.Update(invitation);
            updatedSuccessfully &= await _repository.Update(league);
            await InvalidateLeaguesListCache(teamId);
        }

        return updatedSuccessfully;
    }
    public async Task<bool> DeclineInvitation(Guid leagueId, Guid teamId)
    {
        bool updatedSuccessfully = false;
        var invitation = await _invitationsRepository.Get(leagueId, teamId);
        if (invitation != null)
        {
            invitation.Decline();
            updatedSuccessfully = await _invitationsRepository.Update(invitation);
        }

        return updatedSuccessfully;
    }

    public Task<bool> Validate(CreateLeagueDto dto)
    {
        return _repository.IsUniqueName(dto.Name, DateTimeOffset.UtcNow.Year);
    }

    private Task<List<LeagueListDto>> GetLeaguesByMembership(Guid teamId)
    {
        return _repository.List(teamId);
    }
    private async Task<LeagueDto> GetLeagueDetails(Guid leagueId, Guid teamId, Guid userId)
    {
        var domainModel = await  _repository.Get(leagueId);
        return ToDto(domainModel, teamId, userId);
    }

    private static bool IsOwnerOrMaintainer(Guid userId, Guid teamId, League league)
    {
        var membership = league.Members.FirstOrDefault(m => m.TeamId == teamId);
        return league.OwnerId == userId || membership is {IsMaintainer: true};
    }


    private static LeagueDto ToDto(League league, Guid teamId, Guid userId)
    {
        return new LeagueDto
        {
            Id = league.Id,
            Name = league.Name,
            IsMaintainer = IsOwnerOrMaintainer(userId, teamId, league),
            Members = league.Members.Select(m => new LeagueMemberDto
                { TeamId = m.TeamId, IsMaintainer = m.IsMaintainer }).ToList()
        };
    }

    private async Task InvalidateLeaguesListCache(Guid teamId)
    {
        var cachekey = $"{CacheKeyPrefixes.LeaguesByTeam}{teamId}";
        await InvalidateCache(cachekey);
    }
    private async Task InvalidateLeaguesDetailsCache(Guid leagueId)
    {
        var cachekey = $"{CacheKeyPrefixes.LeagueDetails}{leagueId}";
        await InvalidateCache(cachekey);
    }

    public LeaguesService(
        ILeaguesRepository repository,
        ILeagueInvitationsRepository invitationsRepository,
        ILeaguesDomainService domainService,
        ILogger<LeaguesService> logger,
        IOptions<LeaguesOptions> options
        ) : base(options.Value.CacheConnectionString, logger)
    {
        _repository = repository;
        _invitationsRepository = invitationsRepository;
        _domainService = domainService;
        _logger = logger;
    }


}