using System;
using System.Threading.Tasks;
using F1Manager.Teams.DataTransferObjects;

namespace F1Manager.Teams.Abstractions
{
    public interface IDriversReadRepository
    {
        Task<DriverDto> GetById(Guid id);
    }
}