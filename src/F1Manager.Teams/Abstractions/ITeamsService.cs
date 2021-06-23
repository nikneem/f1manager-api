using System;
using System.Threading.Tasks;
using F1Manager.Shared.DataTransferObjects;
using F1Manager.Teams.DataTransferObjects;

namespace F1Manager.Teams.Abstractions
{
    public interface ITeamsService
    {
        Task<CollectionResult<TeamListItemDto>> List( TeamsListFilterDto filter);
        Task<TeamDetailsDto> Create(Guid userId, TeamCreateDto dto);
        Task<bool> IsUniqueName( Guid userId, string name);
        Task<TeamDetailsDto> GetByUserId( Guid userId);
        Task<TeamDetailsDto> GetById(Guid teamId, Guid userId);
        Task<TeamDetailsDto> Update(Guid userId, Guid id, TeamUpdateDto dto);
        Task<TeamDriverDetailsDto> GetFirstDriver(Guid teamId);
        Task<TeamDriverDetailsDto> GetSecondDriver(Guid teamId);
        Task<TeamDriverDetailsDto> BuyFirstDriver(Guid userId, Guid driverId);
        Task<TeamDriverDetailsDto> BuySecondDriver(Guid userId, Guid driverId);
        Task<SellConfirmationDto> SellDriverConfirmation(Guid userId, Guid teamDriverId);
        Task<bool> SellDriver(Guid userId, Guid teamDriverId);
        Task<TeamEngineDetailsDto> GetEngine(Guid teamId);
        Task<TeamEngineDetailsDto> PurchaseEngine(Guid userId, Guid engineId);
        Task<SellConfirmationDto> SellEngineConfirmation(Guid userId, Guid teamEngineId);
        Task<bool> SellEngine(Guid userId, Guid teamEngineId);
        Task<TeamChassisDetailsDto> GetChassis(Guid teamId);
        Task<TeamChassisDetailsDto> PurchaseChassis(Guid userId,  Guid chassisId);
        Task<SellConfirmationDto> SellChassisConfirmation(Guid userId, Guid teamChassisId);
        Task<bool> SellChassis(Guid userId, Guid teamChassisId);
    }
}