using System;
using System.Threading.Tasks;
using F1Manager.Email.Abstractions;
using F1Manager.Email.Enums;
using F1Manager.Users.Abstractions;
using HexMaster.Email.DomainModels;

namespace F1Manager.Users.Services
{
    public class UsersDomainService : IUsersDomainService
    {
        private readonly IUsersRepository _repository;
        private readonly IMailDispatcher _mailDispatcher;

        public Task<bool> GetIsUsernameUnique(Guid id, string username)
        {
            return _repository.GetIsUsernameUnique(id, username);
        }

        public Task<bool> GetIsEmailAddressUnique(Guid id, string emailAddress)
        {
            return _repository.GetIsEmailAddressUnique(id, emailAddress);
        }

        public Task<bool> PasswordIsReset(
            string baseUrl,
            string emailAddress, 
            string usernameOrEmail, 
            string password,
            string passwordVerificationCode)
        {
            var mailRecipient = new Recipient(emailAddress);
            mailRecipient.AddSubstitution("{{baseurl}}", baseUrl);
            mailRecipient.AddSubstitution("{{useroremail}}", usernameOrEmail);
            mailRecipient.AddSubstitution("{{email}}", emailAddress);
            mailRecipient.AddSubstitution("{{emailAddress}}", emailAddress);
            mailRecipient.AddSubstitution("{{verificationcode}}", passwordVerificationCode);
            mailRecipient.AddSubstitution("{{password}}", password);

            return _mailDispatcher.Dispatch(Subjects.ResetPassword, "en", mailRecipient);
        }

        public Task<bool> PasswordChanged(string emailAddress, string ipAddress)
        {
            var mailRecipient = new Recipient(emailAddress);
            mailRecipient.AddSubstitution("{{ipaddress}}", ipAddress);
            mailRecipient.AddSubstitution("{{email}}", emailAddress);
            mailRecipient.AddSubstitution("{{emailaddress}}", emailAddress);
            mailRecipient.AddSubstitution("{{datetime}}", DateTimeOffset.UtcNow.ToString("F"));

            return _mailDispatcher.Dispatch(Subjects.PasswordChanged, "en", mailRecipient);
        }

        public UsersDomainService(
            IUsersRepository repository,
            IMailDispatcher mailDispatcher)
        {
            _repository = repository;
            _mailDispatcher = mailDispatcher;
        }
    }
}