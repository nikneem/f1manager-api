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
using F1Manager.Shared.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace F1Manager.Leagues.Services;

public class LeaguesService : CachedServiceBase<LeaguesService>, ILeaguesService
{
    private readonly ILeaguesRepository _repository;
    private readonly ILeagueInvitationsRepository _invitationsRepository;
    private readonly ILeagueRequestsRepository _requestsRepository;
    private readonly ILeaguesDomainService _domainService;
    private readonly ILogger<LeaguesService> _logger;

    public async Task<CollectionResult<LeagueListDto>> List(LeaguesListFilterDto filter)
    {
        var cachekey = $"{CacheKeyPrefixes.LeagueAll}";
        var leagueDetails = await GetFromCache(cachekey, () => _repository.List());
        if (!string.IsNullOrWhiteSpace(filter?.Name))
        {
            leagueDetails = leagueDetails
                .Where(l => l.Name.Contains(filter.Name, StringComparison.InvariantCultureIgnoreCase))
                .ToList();
        }
        return new CollectionResult<LeagueListDto>
        {
            Page = 1,
            PageSize = 10,
            TotalEntries = 100,
            TotalPages = 100,
            Entities = leagueDetails
        };
    }

    public Task<List<LeagueListDto>> List(Guid teamId)
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
        _logger.LogInformation("About to create a new league for user {userId} and team {teamId}", userId, teamId);
        var league = await League.Create(userId, dto.Name, _domainService);
        league.AddMember(teamId);
        if (await _repository.Create(league))
        {
            await InvalidateLeaguesListCache(teamId);
            await InvalidateAllLeaguesList();
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
            await InvalidateAllLeaguesList();
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

    public async Task<List<LeagueRequestDto>> ListRequests(Guid leagueId, Guid userId, Guid teamId)
    {
        var league = await _repository.Get(leagueId);
        if (IsOwnerOrMaintainer(userId, teamId, league))
        {
            var leagueRequests = await _requestsRepository.List(leagueId);
            return leagueRequests.Select(dm => new LeagueRequestDto
            {
                TeamId = dm.TeamId,
                CreatedOn = dm.CreatedOn,
                ExpiresOn = dm.ExpiresOn,
            }).ToList();
        }

        return new List<LeagueRequestDto>();
    }

    public async Task<bool> Request(Guid leagueId, Guid userId, Guid teamId)
    {
        var league = await _repository.Get(leagueId);
        if (league.Members.Any(m => m.TeamId == teamId))
        {
            throw new F1ManagerLeaguesException(LeagueErrorCode.AlreadyMemberOfLeague,
                "This team is already member of this league");
        }

        var request = new LeagueRequest(leagueId, teamId);
        try
        {
            return await _requestsRepository.Create(request);
        }
        catch (Exception ex)
        {
            throw new F1ManagerLeaguesException(LeagueErrorCode.RequestAlreadyPending,
                "This team is already member of this league");
            //_logger.LogError(ex, "Failed to create an invitation for league {leagueId} and team {teamId}", leagueId, teamId);
            //return false;
        }
    }

    public async Task<bool> AcceptRequest(Guid leagueId, Guid userId, Guid teamId, Guid requestTeamId)
    {
        bool updatedSuccessfully = false;
        var league = await _repository.Get(leagueId);
        var request = await _requestsRepository.Get(leagueId, requestTeamId);
        if (IsOwnerOrMaintainer(userId, teamId, league))
        {
            request.Accept();
            league.AddMember(request.TeamId);
            updatedSuccessfully = await _requestsRepository.Update(request);
            updatedSuccessfully &= await _repository.Update(league);
            await InvalidateLeaguesListCache(teamId);
            await InvalidateAllLeaguesList();
        }

        return updatedSuccessfully;
    }

    public async Task<bool> DeclineRequest(Guid leagueId, Guid userId, Guid teamId, Guid requestTeamId)
    {
        bool updatedSuccessfully = false;
        var league = await _repository.Get(leagueId);
        var request = await _requestsRepository.Get(leagueId, requestTeamId);
        if (IsOwnerOrMaintainer(userId, teamId, league))
        {
            request.Decline();
            updatedSuccessfully = await _requestsRepository.Update(request);
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

    private async Task InvalidateAllLeaguesList()
    {
        var cachekey = $"{CacheKeyPrefixes.LeagueAll}";
        await InvalidateCache(cachekey);
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
        ILeagueRequestsRepository requestsRepository,
        ILeaguesDomainService domainService,
        ILogger<LeaguesService> logger,
        IOptions<LeaguesOptions> options
        ) : base(options.Value.CacheConnectionString, logger)
    {
        _repository = repository;
        _invitationsRepository = invitationsRepository;
        _requestsRepository = requestsRepository;
        _domainService = domainService;
        _logger = logger;
    }


}