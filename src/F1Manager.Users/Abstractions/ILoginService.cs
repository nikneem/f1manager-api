using System.Threading.Tasks;
using F1Manager.Users.DataTransferObjects;

namespace F1Manager.Users.Abstractions
{
    public interface ILoginService
    {

        Task<LoginAttemptDto> RequestLogin();
        Task<UserLoginRequestDto> ValidateLogin(UserLoginRequestDto id);
    }
}
