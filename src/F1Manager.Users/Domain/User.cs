using System;
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

        public string EmailAddress { get; private set; }
        public string EmailVerificationCode { get; private set; }
        public DateTimeOffset? DateEmailVerified { get; private set; }
        public DateTimeOffset DueDateEmailVerified { get; private set; }

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
        public void SetPassword(string value)
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

            Password = Password.Create(value);
            SetState(TrackingState.Modified);
        }
        public void SetEmailAddress(string value)
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

            if (!Equals(EmailAddress, value))
            {
                EmailAddress = value;
                EmailVerificationCode = Randomize.GenerateEmailVerificationCode();
                DateEmailVerified = null;
                DueDateEmailVerified = DateTimeOffset.UtcNow.AddDays(Defaults.EmailVerificationPeriodInDays);
                SetState(TrackingState.Modified);
            }
        }

        public User(Guid id,
            string displayName,
            string username,
            string password,
            string salt,
            string emailAddress,
            DateTimeOffset? emailVerified,
            DateTimeOffset dueEmailVerification,
            string lockoutReason,
            bool isAdmin,
            DateTimeOffset registerdOn,
            DateTimeOffset? lastLogin) : base(id)
        {
            DisplayName = displayName;
            Username = username;
            Password = new Password(password, salt);
            EmailAddress = emailAddress;
            DateEmailVerified = emailVerified;
            DueDateEmailVerified = dueEmailVerification;
            LockoutReason = lockoutReason;
            IsAdministrator = isAdmin;
            RegisteredOn = registerdOn;
            LastLoginOn = lastLogin;
        }

        internal User() : base(Guid.NewGuid(), TrackingState.New)
        {
            DueDateEmailVerified = DateTimeOffset.UtcNow.AddDays(Defaults.EmailVerificationPeriodInDays);
            RegisteredOn = DateTimeOffset.UtcNow;
        }

        public static async Task< User> Create(string username, string password, string emailAddress, IUsersDomainService domainService)
        {
            var user = new User();
            user.SetDisplayName(username);
            await user.SetUsername(username, domainService);
            user.SetPassword(password);
            user.SetEmailAddress(emailAddress);
            return user;
        }

    }
}
