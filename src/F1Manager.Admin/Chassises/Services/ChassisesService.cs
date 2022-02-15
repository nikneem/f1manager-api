using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Chassises.Abstractions;
using F1Manager.Admin.Chassises.DataTransferObjects;
using F1Manager.Admin.Chassises.DomainModels;
using F1Manager.Admin.Configuration;
using F1Manager.Admin.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace F1Manager.Admin.Chassises.Services
{
    public sealed class ChassisesService : CachedServiceBase<ChassisesService>, IChassisesService
    {
        private readonly IChassisesRepository _chassisRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Task<List<ChassisDetailsDto>> GetActive()
        {
            var cacheKey = CacheKeyPrefixes.ActiveEnginesList;
            return GetFromCache(cacheKey, GetActiveEnginesFromTableStorage);
        }

        private Task<List<ChassisDetailsDto>> GetActiveEnginesFromTableStorage()
        {
            return _chassisRepository.GetActive();
        }

        public Task<List<ChassisDetailsDto>> List(ChassisListFilterDto filter)
        {
            return _chassisRepository.GetList(filter);
        }

        public Task<ChassisDetailsDto> Get(Guid id)
        {
            return _chassisRepository.GetById(id);
        }

        public async Task<ChassisDetailsDto> Create(ChassisDetailsDto dto)
        {
            AssertPlayerIsAdministrator();
            var domainModel = new Chassis();
            MapDtoToDomainModel(dto, domainModel);
            if (await _chassisRepository.Create(domainModel))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveChassisList);
                return DomainModelToDto(domainModel);
            }

            return null;
        }

        public async Task<ChassisDetailsDto> Update(Guid id, ChassisDetailsDto dto)
        {
            AssertPlayerIsAdministrator();
            var domainModel = await _chassisRepository.Get(id);
            MapDtoToDomainModel(dto, domainModel);
            if (await _chassisRepository.Update(domainModel))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveChassisList);
                await InvalidateCache($"{CacheKeyPrefixes.Chassis}-{id}");
                return DomainModelToDto(domainModel);
            }

            return null;
        }

        public async Task<bool> Delete(Guid id)
        {
            AssertPlayerIsAdministrator();
            var domainModel = await _chassisRepository.Get(id);
            domainModel.Delete();
            if (await _chassisRepository.Update(domainModel))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveChassisList);
                await InvalidateCache($"{CacheKeyPrefixes.Chassis}-{id}");
                return true;
            }

            return false;
        }

        public async Task<bool> Undelete(Guid id)
        {
            AssertPlayerIsAdministrator();
            var domainModel = await _chassisRepository.Get(id);
            domainModel.Undelete();
            if (await _chassisRepository.Update(domainModel))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveChassisList);
                await InvalidateCache($"{CacheKeyPrefixes.Chassis}-{id}");
                return true;
            }

            return false;
        }

        private static void MapDtoToDomainModel(ChassisDetailsDto dto, Chassis domainModel)
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

        private static ChassisDetailsDto DomainModelToDto(Chassis domainModel)
        {
            return new ChassisDetailsDto
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
            if (claimsPrincipal != null)
            {
                if (claimsPrincipal.HasClaim("admin", "true"))
                {
                    return;
                }
            }

            throw new F1ManagerMaintenanceException(MaintenanceErrorCode.UserIsNotAnAdmin,
                "The current logged in user is not an administrator");
        }



        public ChassisesService(IChassisesRepository chassisRepository,
            ILogger<ChassisesService> logger,
            IOptions<AdminOptions> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options.Value.CacheConnectionString, logger)
        {
            _chassisRepository = chassisRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}