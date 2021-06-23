using System.Net;
using System.Threading.Tasks;
using F1Manager.Users.DataTransferObjects;

namespace F1Manager.Users.Abstractions
{
    public interface IUsersService
    {
        Task<UserLoginResponseDto> Register(UserRegistrationDto dto, IPAddress origin);
    }
}