using System;
using System.Threading.Tasks;
using F1Manager.Teams.DataTransferObjects;

namespace F1Manager.Teams.Abstractions
{
    public interface ITeamsDomainService
    {
        Task<DriverDto> GetDriverById(Guid id);
        Task<EngineDto> GetEngineById(Guid id);
        Task<ChassisDto> GetChassisById(Guid id);
        Task<bool> IsUniqueName(Guid id, string value);
    }
}