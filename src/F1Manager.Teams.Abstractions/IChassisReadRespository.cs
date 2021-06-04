using System;
using System.Threading.Tasks;
using F1Manager.Teams.DataTransferObjects;

namespace F1Manager.Teams.Abstractions
{
    public interface IChassisReadRespository
    {
        Task<ChassisDto> GetById(Guid id);
    }
}