using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Chassises.DataTransferObjects;

namespace F1Manager.Admin.Chassises.Abstractions
{
    public interface IChassisesService
    {
        Task<List<ChassisDetailsDto>> GetActive();
        Task<List<ChassisDetailsDto>> List(ChassisListFilterDto filter);
        Task<ChassisDetailsDto> Get(Guid id);
        Task<ChassisDetailsDto> Create(ChassisDetailsDto dto);
        Task<ChassisDetailsDto> Update(Guid id, ChassisDetailsDto dto);
        Task<bool> Delete(Guid id);
        Task<bool> Undelete(Guid id);
    }
}