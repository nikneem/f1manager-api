using System;
using System.Net;
using System.Threading.Tasks;
using F1Manager.Users.DataTransferObjects;

namespace F1Manager.Users.Abstractions;

public interface IUsersService
{
    Task<UserLoginResponseDto> Register(UserRegistrationDto dto, IPAddress origin);
    Task<bool> ResetPassword(ResetPasswordDto dto, IPAddress ipAddress);
    Task<bool> VerifyPasswordReset(ResetPasswordVerificationDto dto, IPAddress ipAddress);
    Task<bool> ChangePassword(Guid userId, ChangePasswordDto dto, IPAddress ipAddress);
}