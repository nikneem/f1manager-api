using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Drivers.DataTransferObjects;
using F1Manager.Admin.Drivers.DomainModels;

namespace F1Manager.Admin.Drivers.Abstractions
{
    public interface IDriversRepository
    {
        Task<List<DriverDetailsDto>> GetActive();
        Task<List<DriverDetailsDto>> GetList(DriversListFilterDto filter);
        Task<Driver> Get(Guid id);
        Task<DriverDetailsDto> GetById(Guid id);
        Task<bool> Create(Driver domainModel);
        Task<bool> Update(Driver domainModel);
    }
}