﻿using System;
using System.Net;
using System.Threading.Tasks;
using F1Manager.Email.Abstractions;
using F1Manager.Email.Enums;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.DataTransferObjects;
using F1Manager.Users.Domain;
using F1Manager.Users.Exceptions;
using HexMaster.Email.DomainModels;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace F1Manager.Users.Services
{
    public sealed class UsersService: IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly ILoginsService _loginService;
        private readonly IUsersDomainService _domainService;
        private readonly ILogger<UsersService> _logger;
        private readonly IOptions<UsersOptions> _userOptions;
        private readonly IMailDispatcher _mailDispatcher;

        public async Task<UserLoginResponseDto> Register(UserRegistrationDto dto, IPAddress origin)
        {
            _logger.LogInformation("Handling request to register new user '{username}'", dto.Username);
            var userDomainModel =  await User.Create(dto.Username, dto.Password, dto.EmailAddress, _domainService);
            if (await _repository.Insert(userDomainModel))
            {
                
                var mailRecipient = new Recipient(dto.EmailAddress);
                mailRecipient.AddSubstitution("{{username}}", dto.Username);
                mailRecipient.AddSubstitution("{{email}}", dto.EmailAddress);
                mailRecipient.AddSubstitution("{{emailaddress}}", dto.EmailAddress);
                mailRecipient.AddSubstitution("{{verificationCode}}", userDomainModel.EmailAddress.VerificationCode);
               
                var mailSent = await _mailDispatcher.Dispatch(Subjects.Subscription, "en", mailRecipient);
                if (!mailSent)
                {
                    _logger.LogError("Registration confirmation email was not sent succesfully to {emailaddress}", dto.EmailAddress);
                }

                _logger.LogInformation("New user '{username}' created in the system", dto.Username);
                return await _loginService.GenerateUserLoginSuccessResponse(userDomainModel, origin);
            }

            _logger.LogError("Failed to register user '{username}' as new user due to an unknown reason", dto.Username);
            throw new F1ManagerUserException(UserErrorCode.UserRegistrationFailed, "Failed to register new user");
        }

        public async Task<bool> ResetPassword(ResetPasswordDto dto, IPAddress ipAddress)
        {
            _logger.LogInformation("A password reset request for user {usernameOrEmail} was received from IP Address {ipAddress}", dto.UsernameOrEmail, ipAddress.ToString());
            var user = await _repository.GetByUsername(dto.UsernameOrEmail) ?? 
                       await _repository.GetByEmailAddress(dto.UsernameOrEmail);

            if (user != null && await user.ResetPassword(dto.BaseUrl, _domainService))
            {
                return await _repository.Update(user);
            }

            return true;
        }

        public async Task<bool> VerifyPasswordReset(ResetPasswordVerificationDto dto, IPAddress ipAddress)
        {
            var user = await _repository.GetByUsername(dto.UsernameOrEmail) ??
                       await _repository.GetByEmailAddress(dto.UsernameOrEmail);

            if (user != null && await user.VerifyNewPassword(dto.VerificationCode, ipAddress.ToString(), _domainService))
            {
                return await _repository.Update(user);
            }

            return false;
        }

        public async Task<bool> ChangePassword(Guid userId, ChangePasswordDto dto, IPAddress ipAddress)
        {
            var user = await _repository.GetById(userId);
            if (user.Password.Validate(dto.OldPassword) && await user.SetPassword(dto.NewPassword, ipAddress.ToString(), _domainService))
            {
                return await _repository.Update(user);
            }

            return false;
        }


        private static UserDetailsDto ToUserDetailsDto(User domainModel)
        {
            return new UserDetailsDto
            {
                Id = domainModel.Id,
                DisplayName = domainModel.DisplayName,
                Username = domainModel.Username,
                EmailAddress = domainModel.EmailAddress.Value,
                DateEmailVerified = domainModel.EmailAddress.VerificationOn,
                RegisteredOn = domainModel.RegisteredOn,
                LastLoginOn = domainModel.LastLoginOn
            };
        }

        
        public UsersService(IUsersRepository repository,
            ILoginsService loginService,
            IUsersDomainService domainService,
            ILogger<UsersService> logger,
            IOptions<UsersOptions> userOptions,
            IMailDispatcher mailDispatcher
            )
        {
            _repository = repository;
            _loginService = loginService;
            _domainService = domainService;
            _logger = logger;
            _userOptions = userOptions;
            _mailDispatcher = mailDispatcher;
        }
    }
}
