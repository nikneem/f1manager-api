using System.Threading.Tasks;

namespace F1Manager.Leagues.Abstractions
{
    public interface ILeaguesDomainService
    {
        Task<bool> IsUniqueName(string name, int season);
    }
}