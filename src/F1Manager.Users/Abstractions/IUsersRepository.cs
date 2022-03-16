using System;
using System.Threading.Tasks;
using F1Manager.Users.Domain;

namespace F1Manager.Users.Abstractions;

public interface IUsersRepository
{
    Task<bool> GetIsUsernameUnique(Guid id, string username);
    Task<bool> GetIsEmailAddressUnique(Guid id, string emailAddress);
    Task<User> GetByUsername(string username);
    Task<User> GetByEmailAddress(string emailAddress);
    Task<bool> Insert(User userDomainModel);
    Task<bool> Update(User userDomainModel);
    Task<User> GetById(Guid userId);
}