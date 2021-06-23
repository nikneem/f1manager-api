using System;
using System.Net;
using System.Threading.Tasks;
using F1Manager.Users.DataTransferObjects;
using F1Manager.Users.Domain;

namespace F1Manager.Users.Abstractions
{
    public interface ILoginsService
    {

        Task<LoginAttemptDto> RequestLogin();
        Task<UserLoginResponseDto> Login(UserLoginRequestDto dto, IPAddress ipAddress);
        Task<UserLoginResponseDto> Refresh(string token, IPAddress ipAddress);
        Task<UserLoginResponseDto> GenerateUserLoginSuccessResponse(User user, IPAddress ipAddress);
    }
}
