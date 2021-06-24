using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Configuration;
using F1Manager.Admin.Drivers.Abstractions;
using F1Manager.Admin.Drivers.DataTransferObjects;
using F1Manager.Admin.Drivers.DomainModels;
using F1Manager.Admin.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.Helpers;
using F1Manager.Shared.ValueObjects;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace F1Manager.Admin.Drivers.Services
{
    public class DriversService : CachedServiceBase<DriversService>, IDriversService
    {
        private readonly IDriversRepository _driversRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Task<List<DriverDetailsDto>> GetActive()
        {
            var cachekey = CacheKeyPrefixes.ActiveDriversList;
            return GetFromCache(cachekey, GetActiveDriversFromTableStorage);
        }

        private Task<List<DriverDetailsDto>> GetActiveDriversFromTableStorage()
        {
            return _driversRepository.GetActive();
        }

        public Task<List<DriverDetailsDto>> List(DriversListFilterDto filter)
        {
            return _driversRepository.GetList(filter);
        }

        public Task<DriverDetailsDto> Get(Guid id)
        {
            return _driversRepository.GetById(id);
        }

        public async Task<DriverDetailsDto> Create(DriverDetailsDto dto)
        {
             AssertPlayerIsAdministrator();
            var driver = new Driver();
            MapDtoToDomainModel(dto, driver);
            if (await _driversRepository.Create(driver))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveDriversList);
                return DomainModelToDto(driver);
            }

            return null;
        }

        public async Task<DriverDetailsDto> Update(Guid id, DriverDetailsDto dto)
        {
             AssertPlayerIsAdministrator();
            var driver = await _driversRepository.Get(id);
            MapDtoToDomainModel(dto, driver);
            if (await _driversRepository.Update(driver))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveDriversList);
                await InvalidateCache($"{CacheKeyPrefixes.Drivers}-{id}");
                return DomainModelToDto(driver);
            }

            return null;
        }

        public async Task<bool> Delete(Guid id)
        {
             AssertPlayerIsAdministrator();
            var driver = await _driversRepository.Get(id);
            driver.Delete();
            if (await _driversRepository.Update(driver))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveDriversList);
                await InvalidateCache($"{CacheKeyPrefixes.Drivers}-{id}");
                return true;
            }

            return false;
        }

        public async Task<bool> Undelete(Guid id)
        {
             AssertPlayerIsAdministrator();
            var driver = await _driversRepository.Get(id);
            driver.Undelete();
            if (await _driversRepository.Update(driver))
            {
                await InvalidateCache(CacheKeyPrefixes.ActiveDriversList);
                await InvalidateCache($"{CacheKeyPrefixes.Drivers}-{id}");
                return true;
            }

            return false;
        }

        private static void MapDtoToDomainModel(DriverDetailsDto dto, Driver driver)
        {
            driver.SetName(dto.Name);
            driver.SetDateOfBirth(dto.DateOfBirth);
            driver.SetCountryOfOrigin(dto.Country);
            driver.SetPictureUrl(dto.PictureUrl);
            driver.SetAvailable(dto.IsAvailable);
            driver.SetCurrentValue(dto.Value);
            driver.SetActiveDates(new DateRange(dto.ActiveFrom, dto.ActiveUntil));
        }

        private static DriverDetailsDto DomainModelToDto(Driver driver)
        {
            return new DriverDetailsDto
            {
                Id = driver.Id,
                Name = driver.Name,
                Country = driver.Country,
                DateOfBirth = driver.DateOfBirth,
                ActiveFrom = driver.ActiveFrom,
                ActiveUntil = driver.ActiveUntil,
                Value = driver.Value,
                PictureUrl = driver.PictureUrl,
                IsAvailable = driver.IsAvailable,
                IsDeleted = driver.IsDeleted
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



        public DriversService(IDriversRepository driversRepository, 
            ILogger<DriversService> logger,
            IOptions<AdminOptions> options,
            IHttpContextAccessor httpContextAccessor)
            : base(options.Value.CacheConnectionString, logger)
        {
            _driversRepository = driversRepository;
            _httpContextAccessor = httpContextAccessor;
        }
    }
}
