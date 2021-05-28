using System;
using System.Threading.Tasks;

namespace F1Manager.Users.Abstractions
{
    public interface IUsersDomainService
    {
        Task<bool> GetIsUsernameUnique(Guid id, string username);
    }
}