using System.Threading.Tasks;
using F1Manager.Users.DataTransferObjects;

namespace F1Manager.Users.Abstractions
{
    public interface IUsersService
    {
        Task<UserDetailsDto> Register(UserRegistrationDto dto);
        Task<UserLoginResponseDto> Login(UserLoginRequestDto dto);
    }
}