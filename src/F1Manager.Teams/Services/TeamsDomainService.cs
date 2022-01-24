using System;
using System.Threading.Tasks;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.Configuration;
using F1Manager.Teams.DataTransferObjects;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace F1Manager.Teams.Services
{


    public class TeamsDomainService : CachedServiceBase<TeamsDomainService>, ITeamsDomainService
    {
        private readonly IDriversReadRepository _driversRepository;
        private readonly IEnginesReadRespository _enginesRepository;
        private readonly IChassisReadRespository _chassisRepository;
        private readonly ITeamsRepository _teamsRepository;

        public Task<DriverDto> GetDriverById(Guid id)
        {
            var cacheKey = $"{CacheKeyPrefixes.Drivers}-{id}";
            return GetFromCache(cacheKey, () => _driversRepository.GetById(id));
        }

        public Task<EngineDto> GetEngineById(Guid id)
        {
            var cacheKey = $"{CacheKeyPrefixes.Engines}-{id}";
            return GetFromCache(cacheKey, () => _enginesRepository.GetById(id));
        }

        public Task<ChassisDto> GetChassisById(Guid id)
        {
            var cacheKey = $"{CacheKeyPrefixes.Chassis}-{id}";
            return GetFromCache(cacheKey, () => _chassisRepository.GetById(id));
        }

        public Task<bool> IsUniqueName(Guid id, string value)
        {
            return _teamsRepository.IsUniqueName(id, value);
        }

        public TeamsDomainService(
            IDriversReadRepository driversRepository,
            IEnginesReadRespository enginesRepository,
            IChassisReadRespository chassisRepository,
            ITeamsRepository teamsRepository,
            ILogger<TeamsDomainService> logger,
            IOptions<TeamsOptions> options) : base(options.Value.CacheConnectionString, logger)
        {
            _driversRepository = driversRepository;
            _enginesRepository = enginesRepository;
            _chassisRepository = chassisRepository;
            _teamsRepository = teamsRepository;
        }

    }
}
