using System;
using System.Threading.Tasks;
using F1Manager.Shared.DataTransferObjects;
using F1Manager.Teams.DataTransferObjects;
using F1Manager.Teams.Domain;

namespace F1Manager.Teams.Abstractions
{
    public interface ITeamsRepository
    {
        Task<CollectionResult<TeamListItemDto>> GetList(int seasonId, TeamsListFilterDto filter);
        Task<Team> Get(Guid teamId);
        Task<Team> GetByUserId(int getSeasonId, Guid userId);
        Task<Guid?> GetTeamId(int seasonId, Guid playerId);
        Task<bool> Create(Team team);
        Task<bool> Update(Team team);
        Task<bool> GetUserHasTeam(int seasonId, Guid userId);
        Task<bool> IsUniqueName(Guid id, string name);
    }
}