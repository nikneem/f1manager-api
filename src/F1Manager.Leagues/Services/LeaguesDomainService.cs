using System;
using System.Threading.Tasks;
using F1Manager.Leagues.Abstractions;

namespace F1Manager.Leagues.Services
{
    public class LeaguesDomainService: ILeaguesDomainService
    {
        private readonly ILeaguesRepository _leaguesRepository;

        public Task<bool> IsUniqueName(string name, int season)
        {
            return _leaguesRepository.IsUniqueName(name, season);
        }

        public LeaguesDomainService(ILeaguesRepository leaguesRepository)
        {
            _leaguesRepository = leaguesRepository;
        }
    }
}
