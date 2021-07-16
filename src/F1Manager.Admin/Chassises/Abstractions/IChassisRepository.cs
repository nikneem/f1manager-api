using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Chassises.DataTransferObjects;
using F1Manager.Admin.Chassises.DomainModels;

namespace F1Manager.Admin.Chassises.Abstractions
{
    public interface IChassisesRepository
    {
        Task<List<ChassisDetailsDto>> GetActive();
        Task<List<ChassisDetailsDto>> GetList(ChassisListFilterDto filter);
        Task<Chassis> Get(Guid id);
        Task<ChassisDetailsDto> GetById(Guid id);
        Task<bool> Create(Chassis domainModel);
        Task<bool> Update(Chassis domainModel);
    }
}