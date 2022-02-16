using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Configuration;
using F1Manager.Admin.Engines.Abstractions;
using F1Manager.Admin.Engines.DataTransferObjects;
using F1Manager.Admin.Engines.DomainModels;
using F1Manager.Admin.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace F1Manager.Admin.Engines.Services
{
    public sealed class EnginesService : CachedServiceBase<EnginesService>, IEnginesService
    {
        private readonly IEnginesRepository _enginesRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Task<List<EngineDetailsDto>> GetActive()
        {
            var cacheKey = CacheKeyPrefixes.ActiveEnginesList;
            return GetFromCache(cacheKey, GetActiveEnginesFromTableStorage);
        }

        private Task<List<EngineDetailsDto>> GetActiveEnginesFromTableStorage()
        {
            return _enginesRepository.GetActive();
        }

        public Task<List<EngineDetailsDto>> List(EnginesListFilterDto filter)
        {
            return _enginesRepository.GetList(filter);
        }

        public Task<EngineDetailsDto> Get(Guid id)
        {
            return _enginesRepository.GetById(id);
        }

        public async Task<EngineDetailsDto> Create(EngineDetailsDto dto)
        {
            AssertPlayerIsAdministrator();
            var domainModel = new Engine();
            MapDtoToDomainModel(dto, domainModel);
            if (await _enginesRepository.Create(domainModel))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveEnginesList);
                return DomainModelToDto(domainModel);
            }

            return null;
        }

        public async Task<EngineDetailsDto> Update(Guid id, EngineDetailsDto dto)
        {
            AssertPlayerIsAdministrator();
            var domainModel = await _enginesRepository.Get(id);
            MapDtoToDomainModel(dto, domainModel);
            if (await _enginesRepository.Update(domainModel))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveEnginesList);
                await InvalidateCache($"{CacheKeyPrefixes.Engines}-{id}");
                return DomainModelToDto(domainModel);
            }

            return null;
        }

        public async Task<bool> Delete(Guid id)
        {
            AssertPlayerIsAdministrator();
            var domainModel = await _enginesRepository.Get(id);
            domainModel.Delete();
            if (await _enginesRepository.Update(domainModel))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveEnginesList);
                await InvalidateCache($"{CacheKeyPrefixes.Engines}-{id}");
                return true;
            }

            return false;
        }

        public async Task<bool> Undelete(Guid id)
        {
            AssertPlayerIsAdministrator();
            var domainModel = await _enginesRepository.Get(id);
            domainModel.Undelete();
            if (await _enginesRepository.Update(domainModel))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveEnginesList);
                await InvalidateCache($"{CacheKeyPrefixes.Engines}-{id}");
                return true;
            }

            return false;
        }

        private static void MapDtoToDomainModel(EngineDetailsDto dto, Engine domainModel)
        {
            domainModel.SetName(dto.Name);
            domainModel.SetPictureUrl(dto.PictureUrl);
            domainModel.SetAvailable(dto.IsAvailable);
            domainModel.SetCurrentValue(dto.Value);
            domainModel.SetManufacturer(dto.Manufacturer);
            domainModel.SetModel(dto.Model);
            domainModel.SetWeeklyWearOff(dto.WeeklyWearOff);
            domainModel.SetMaxWearOff(dto.MaxWearOff);

            domainModel.SetActiveDates(new DateRange(dto.ActiveFrom, dto.ActiveUntil));
        }

        private static EngineDetailsDto DomainModelToDto(Engine domainModel)
        {
            return new EngineDetailsDto
            {
                Id = domainModel.Id,
                Name = domainModel.Name,
                Manufacturer = domainModel.Manufacturer,
                Model = domainModel.Model,
                WeeklyWearOff = domainModel.WeeklyWearOffPercentage,
                MaxWearOff = domainModel.MaxWearOffPercentage,
                ActiveFrom = domainModel.ActiveFrom,
                ActiveUntil = domainModel.ActiveUntil,
                Value = domainModel.Value,
                PictureUrl = domainModel.PictureUrl,
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



        public EnginesService(IEnginesRepository enginesRepository,
            ILogger<EnginesService> logger,
            IOptions<AdminOptions> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options.Value.CacheConnectionString, logger)
        {
            _enginesRepository = enginesRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}