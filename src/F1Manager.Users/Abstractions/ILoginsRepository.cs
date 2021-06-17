using System;
using System.Threading.Tasks;
using F1Manager.Users.Domain;

namespace F1Manager.Users.Abstractions
{
    public interface ILoginsRepository
    {
        Task<bool> RegisterLoginAttempt(LoginAttempt attempt);
        Task<LoginAttempt> ValidateLoginAttempt(Guid loginAttempt);
    }
}