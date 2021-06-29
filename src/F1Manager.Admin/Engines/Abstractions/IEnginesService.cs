using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Engines.DataTransferObjects;

namespace F1Manager.Admin.Engines.Abstractions
{
    public interface IEnginesService
    {
        Task<List<EngineDetailsDto>> GetActive();
        Task<List<EngineDetailsDto>> List(EnginesListFilterDto filter);
        Task<EngineDetailsDto> Get(Guid id);
        Task<EngineDetailsDto> Create(EngineDetailsDto dto);
        Task<EngineDetailsDto> Update(Guid id, EngineDetailsDto dto);
        Task<bool> Delete(Guid id);
        Task<bool> Undelete(Guid id);
    }
}