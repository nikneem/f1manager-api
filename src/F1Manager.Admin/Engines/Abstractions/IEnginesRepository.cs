using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using F1Manager.Admin.Engines.DataTransferObjects;
using F1Manager.Admin.Engines.DomainModels;

namespace F1Manager.Admin.Engines.Abstractions
{
    public interface IChassisRepository
    {
        Task<List<EngineDetailsDto>> GetActive();
        Task<List<EngineDetailsDto>> GetList(EnginesListFilterDto filter);
        Task<Engine> Get(Guid id);
        Task<EngineDetailsDto> GetById(Guid id);
        Task<bool> Create(Engine domainModel);
        Task<bool> Update(Engine domainModel);
    }
}