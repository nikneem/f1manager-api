using System;
using System.Threading.Tasks;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.DataTransferObjects;

namespace F1Manager.Teams.Services
{


    public class TeamsDomainService : ITeamsDomainService
    {
        private readonly IDriversReadRepository _driversRepository;
        private readonly IEnginesReadRespository _enginesRepository;
        private readonly IChassisReadRespository _chassisRepository;
        private readonly ITeamsRepository _teamsRepository;

        public  Task<DriverDto> GetDriverById(Guid id)
        {
            return _driversRepository.GetById(id);
        }

        public Task<EngineDto> GetEngineById(Guid id)
        {
            return _enginesRepository.GetById(id);
        }

        public Task<ChassisDto> GetChassisById(Guid id)
        {
            return _chassisRepository.GetById(id);
        }

        public Task<bool> IsUniqueName(Guid id, string value)
        {
            return _teamsRepository.IsUniqueName(id, value);
        }

        public TeamsDomainService(
            IDriversReadRepository driversRepository, 
            IEnginesReadRespository enginesRepository,
            IChassisReadRespository chassisRepository,
            ITeamsRepository teamsRepository)
        {
            _driversRepository = driversRepository;
            _enginesRepository = enginesRepository;
            _chassisRepository = chassisRepository;
            _teamsRepository = teamsRepository;
        }

    }
}
