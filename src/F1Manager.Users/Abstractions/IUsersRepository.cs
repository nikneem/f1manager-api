using System;
using System.Threading.Tasks;
using F1Manager.Users.Domain;

namespace F1Manager.Users.Abstractions
{
    public interface IUsersRepository
    {
        Task<bool> GetIsUsernameUnique(Guid id, string username);
        Task<User> GetByUsername(string username);
        Task<bool> Insert(User userDomainModel);
        Task<User> GetById(Guid userId);
    }
}