using System;
using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.Enums;
using F1Manager.Shared.Helpers;
using F1Manager.Shared.ValueObjects;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Exceptions;

namespace F1Manager.Users.Domain
{
    public class User : DomainModel<Guid>
    {

        public string DisplayName { get; private set; }
        public string Username { get; private set; }
        public Password Password { get; private set; }

        public Password NewPassword { get; private set; }
        public string NewPasswordVerification { get; private set; }

        public EmailAddress EmailAddress { get; private set; }

        public string LockoutReason { get; private set; }
        public bool IsLockedOut => !string.IsNullOrWhiteSpace(LockoutReason);
        public bool IsAdministrator { get; private set; }

        public DateTimeOffset ActiveFrom { get; private set; }
        public DateTimeOffset ActiveUntil { get; private set; }

        public DateTimeOffset RegisteredOn { get; private set; }
        public DateTimeOffset? LastLoginOn { get; private set; }

        public void SetDisplayName(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerUserException(UserErrorCode.DisplayNameNullOrEmpty,
                    "The user's display name cannot be null or empty");
            }
            if (!Equals(DisplayName, value))
            {
                DisplayName = value;
                SetState(TrackingState.Modified);
            }
        }
        public async Task SetUsername(string value, IUsersDomainService domainService)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerUserException(UserErrorCode.UsernameNullOrEmpty,
                    "The username cannot be null or empty");
            }
            if (!Regex.IsMatch(value, RegularExpressions.Username))
            {
                throw new F1ManagerUserException(UserErrorCode.InvalidUsername,
                    $"The username {value} does not meet the required regular expression");
            }
            if (!await domainService.GetIsUsernameUnique(Id, value))
            {
                throw new F1ManagerUserException(UserErrorCode.UsernameNotUnique,
                    $"The username {value} is already taken by a different user");
            }
            if (!Equals(Username, value))
            {
                Username = value;
                SetState(TrackingState.Modified);
            }
        }
        public Task<bool> SetPassword(string value, string ipAddress, IUsersDomainService domainService)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerUserException(UserErrorCode.PasswordNullOrEmpty,
                    "The password cannot be null or empty");
            }

            if (!Regex.IsMatch(value, RegularExpressions.UpperLowedDigit))
            {
                throw new F1ManagerUserException(UserErrorCode.InvalidPassword,
                    "The password does not meet the required regular expression");
            }
            return SetPassword(Password.Create(value), ipAddress, domainService);
        }
        private Task<bool> SetPassword(Password value, string ipAddress, IUsersDomainService domainService)
        {
            Password = value;
            SetState(TrackingState.Modified);
            return  domainService.PasswordChanged(EmailAddress.Value, ipAddress);
        }
        public async Task SetEmailAddress(string value, IUsersDomainService domainService)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerUserException(UserErrorCode.EmailNullOrEmpty,
                    "The email address cannot be null or empty");
            }

            if (!Regex.IsMatch(value, RegularExpressions.EmailAddress))
            {
                throw new F1ManagerUserException(UserErrorCode.InvalidEmail,
                    $"The email address '{value}' does not meet the required regular expression");
            }

            if (!await domainService.GetIsEmailAddressUnique(Id, value))
            {
                throw new F1ManagerUserException(UserErrorCode.EmailNotUnique,
                    $"The email address {value} is not unique");
            }
            
            if (!Equals(EmailAddress?.Value, value))
            {
                EmailAddress = EmailAddress.Create(value);
                SetState(TrackingState.Modified);
            }
        }

        public async Task<bool> ResetPassword(string baseUrl, IUsersDomainService domainService)
        {
            var newPassword = Randomize.GeneratePassword();
            var password = Password.Create(newPassword);
            NewPassword = password;
            NewPasswordVerification = Randomize.GenerateEmailVerificationCode();
            SetState(TrackingState.Modified);
            return await domainService.PasswordIsReset(baseUrl, EmailAddress.Value, EmailAddress.Value, newPassword, NewPasswordVerification);
        }
        public async Task<bool> VerifyNewPassword(string code, string ipAddress, IUsersDomainService domainService)
        {
            var response = false;

            if (!string.IsNullOrWhiteSpace(NewPasswordVerification) &&
                NewPasswordVerification.Equals(code, StringComparison.InvariantCultureIgnoreCase))
            {
                response = await SetPassword(NewPassword, ipAddress, domainService);
                NewPassword = null;
                NewPasswordVerification = null;
                SetState(TrackingState.Modified);
            }
            return response;
        }


        public User(Guid id,
            string displayName,
            string username,
            string password,
            string salt,
            EmailAddress  emailAddress,
            string lockoutReason,
            bool isAdmin,
            DateTimeOffset registerdOn,
            DateTimeOffset? lastLogin,
            string newPassword,
            string newPasswordSalt,
            string newPasswordVerification) : base(id)
        {
            DisplayName = displayName;
            Username = username;
            Password = new Password(password, salt);
            EmailAddress = emailAddress;
            LockoutReason = lockoutReason;
            IsAdministrator = isAdmin;
            RegisteredOn = registerdOn;
            LastLoginOn = lastLogin;
            NewPassword = new Password(newPassword, newPasswordSalt);
             NewPasswordVerification = newPasswordVerification;
        }

        private User() : base(Guid.NewGuid(), TrackingState.New)
        {
            RegisteredOn = DateTimeOffset.UtcNow;
        }

        public static async Task< User> Create(string username, string password, string emailAddress, IUsersDomainService domainService)
        {
            var user = new User();
            user.SetDisplayName(username);
            await user.SetUsername(username, domainService);
            await user.SetEmailAddress(emailAddress, domainService);
           await  user.SetPassword(password, null, domainService);
            return user;
        }

    }
}
