using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Drivers.DataTransferObjects;

namespace F1Manager.Admin.Drivers.Abstractions
{
    public interface IDriversService
    {
        Task<List<DriverDetailsDto>> GetActive();
        Task<List<DriverDetailsDto>> List(DriversListFilterDto filter);
        Task<DriverDetailsDto> Get(Guid id);
        Task<DriverDetailsDto> Create(DriverDetailsDto dto);
        Task<DriverDetailsDto> Update(Guid id, DriverDetailsDto dto);
        Task<bool> Delete(Guid id);
        Task<bool> Undelete(Guid id);
    }
}