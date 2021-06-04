using System;
using System.Threading.Tasks;
using F1Manager.Teams.Abstractions;

namespace F1Manager.Teams.Repositories
{
    public sealed class TeamsRepository : ITeamsRepository
    {

        public async Task<bool> IsUniqueName(Guid id, string name)
        {
            return false;
        }

    }
}