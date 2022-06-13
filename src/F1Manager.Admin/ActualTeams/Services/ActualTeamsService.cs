using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.ActualTeams.Abstractions;
using F1Manager.Admin.ActualTeams.DataTransferObjects;
using F1Manager.Admin.ActualTeams.DomainModel;
using F1Manager.Admin.Configuration;
using F1Manager.Admin.Engines.DomainModels;
using F1Manager.Admin.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace F1Manager.Admin.ActualTeams.Services;

public class ActualTeamsService: CachedServiceBase<ActualTeamsService>, IActualTeamsService
{
    private readonly IActualTeamsRepository _actualTeamsRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public Task<List<ActualTeamDetailsDto>> GetActive()
    {
        var cacheKey = CacheKeyPrefixes.ActiveActualTeamsList;
        return GetFromCache(cacheKey, GetActiveActualTeamsFromTableStorage);
    }

    public Task<List<ActualTeamDetailsDto>> List(ActualTeamListFilterDto filter)
    {
        return _actualTeamsRepository.GetList(filter);
    }

    public Task<ActualTeamDetailsDto> Get(Guid id)
    {
        return _actualTeamsRepository.GetById(id);
    }

    public async Task<ActualTeamDetailsDto> Create(ActualTeamDetailsDto dto)
    {
        AssertPlayerIsAdministrator();
        var domainModel = ActualTeam.Create(dto.Name,
            dto.Base,
            dto.Principal,
            dto.TechnicalChief,
            dto.FirstDriverId,
            dto.SecondDriverId,
            dto.EngineId,
            dto.ChassisId);
        if (await _actualTeamsRepository.Create(domainModel))
        {
            await InvalidateCache(CacheKeyPrefixes.ActiveActualTeamsList);
            return DomainModelToDto(domainModel);
        }

        return null;
    }

    public async Task<ActualTeamDetailsDto> Update(Guid id, ActualTeamDetailsDto dto)
    {
        AssertPlayerIsAdministrator();
        var domainModel = await _actualTeamsRepository.Get(id);
        MapDtoToDomainModel(dto, domainModel);
        if (await _actualTeamsRepository.Update(domainModel))
        {
            await InvalidateCache(CacheKeyPrefixes.ActiveActualTeamsList);
            return DomainModelToDto(domainModel);
        }

        return null;
    }

    public async Task<bool> Delete(Guid id)
    {
        AssertPlayerIsAdministrator();
        var domainModel = await _actualTeamsRepository.Get(id);
        domainModel.Delete();
        if (await _actualTeamsRepository.Update(domainModel))
        {
            await InvalidateCache(CacheKeyPrefixes.ActiveActualTeamsList);
            return true;
        }

        return false;
    }

    public async Task<bool> Undelete(Guid id)
    {
        AssertPlayerIsAdministrator();
        var domainModel = await _actualTeamsRepository.Get(id);
        domainModel.Undelete();
        if (await _actualTeamsRepository.Update(domainModel))
        {
            await InvalidateCache(CacheKeyPrefixes.ActiveActualTeamsList);
            return true;
        }

        return false;
    }

    private Task<List<ActualTeamDetailsDto>> GetActiveActualTeamsFromTableStorage()
    {
        return _actualTeamsRepository.GetActive();
    }

    private static void MapDtoToDomainModel(ActualTeamDetailsDto dto, ActualTeam domainModel)
    {
        domainModel.SetName(dto.Name);
        domainModel.SetBase(dto.Base);
        domainModel.SetPrincipal(dto.Principal);
        domainModel.SetTechnicalChief(dto.TechnicalChief);
        domainModel.SetFirstDriver(dto.FirstDriverId);
        domainModel.SetSecondDriver(dto.SecondDriverId);
        domainModel.SetEngine(dto.EngineId);
        domainModel.SetChassis(dto.ChassisId);
        domainModel.SetAvailable(dto.IsAvailable);
    }

    private static ActualTeamDetailsDto DomainModelToDto(ActualTeam domainModel)
    {
        return new ActualTeamDetailsDto
        {
            Id = domainModel.Id,
            Name = domainModel.Name,
            Base = domainModel.Base,
            Principal = domainModel.Principal,
            TechnicalChief = domainModel.TechnicalChief,
            FirstDriverId = domainModel.FirstDriverId,
            SecondDriverId = domainModel.SecondDriverId,
            EngineId = domainModel.EngineId,
            ChassisId = domainModel.ChassisId,
            IsAvailable = domainModel.IsAvailable,
            IsDeleted = domainModel.IsDeleted
        };
    }

    private void AssertPlayerIsAdministrator()
    {
        var claimsPrincipal = _httpContextAccessor.HttpContext?.User;
        if (claimsPrincipal != null && claimsPrincipal.HasClaim("admin", "true"))
        {
            return;
        }

        throw new F1ManagerMaintenanceException(MaintenanceErrorCode.UserIsNotAnAdmin,
            "The current logged in user is not an administrator");
    }

    public ActualTeamsService(IActualTeamsRepository actualTeamsRepository,
        ILogger<ActualTeamsService> logger,
        IOptions<AdminOptions> options,
        IHttpContextAccessor httpContextAccessor)

        : base(options.Value.CacheConnectionString, logger)
    {
        _actualTeamsRepository = actualTeamsRepository;
        _httpContextAccessor = httpContextAccessor;
    }
}