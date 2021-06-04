using System;
using System.Threading.Tasks;

namespace F1Manager.Teams.Abstractions
{
    public interface ITeamsRepository
    {
        Task<bool> IsUniqueName(Guid id, string name);
    }
}