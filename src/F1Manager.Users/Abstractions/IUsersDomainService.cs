using System;
using System.Threading.Tasks;

namespace F1Manager.Users.Abstractions
{
    public interface IUsersDomainService
    {
        Task<bool> GetIsUsernameUnique(Guid id, string username);
        Task<bool> GetIsEmailAddressUnique(Guid id, string emailAddress);

        Task<bool> PasswordIsReset(
            string baseUrl,
            string emailAddress,
            string usernameOrEmail,
            string password,
            string passwordVerificationCode);

        Task<bool> PasswordChanged(string emailAddress, string ipAddress);
    }
}