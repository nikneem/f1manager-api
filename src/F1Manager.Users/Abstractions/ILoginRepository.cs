using System.Threading.Tasks;
using F1Manager.Users.DataTransferObjects;

namespace F1Manager.Users.Abstractions
{
    public interface ILoginRepository
    {
        Task<LoginAttemptDto> RegisterLoginAttempt(string key, string iv);
    }
}