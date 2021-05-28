using System.Threading.Tasks;
using F1Manager.Abstractions.Users.DataTransferObjects;

namespace F1Manager.Abstractions.Users.Services
{
    public interface IUsersService
    {
        Task<UserDetailsDto> Register(UserRegistrationDto dto);
    }
}