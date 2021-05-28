using System;
using System.Threading.Tasks;
using F1Manager.Users.Abstractions;

namespace F1Manager.Users.Services
{
    public class UsersDomainService : IUsersDomainService
    {
        private readonly IUsersRepository _repository;

        public Task<bool> GetIsUsernameUnique(Guid id, string username)
        {
            return _repository.GetIsUsernameUnique(id, username);
        }

        public UsersDomainService(IUsersRepository repository)
        {
            _repository = repository;
        }
    }
}