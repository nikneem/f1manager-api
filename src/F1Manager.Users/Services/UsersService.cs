using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.DataTransferObjects;
using F1Manager.Users.Domain;
using F1Manager.Users.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace F1Manager.Users.Services
{
    public sealed class UsersService: IUsersService
    {
        private readonly IUsersRepository _repository;
        private readonly IUsersDomainService _domainService;
        private readonly ILogger<UsersService> _logger;
        private readonly IOptions<UsersOptions> _userOptions;

        public async Task<UserDetailsDto> Register(UserRegistrationDto dto)
        {
            _logger.LogInformation("Handling request to register new user '{username}'", dto.Username);
            var userDomainModel =  await User.Create(dto.Username, dto.Password, dto.EmailAddress, _domainService);
            if (await _repository.Insert(userDomainModel))
            {
                _logger.LogInformation("New user '{username}' created in the system", dto.Username);
                return ToUserDetailsDto(userDomainModel);
            }

            _logger.LogError("Failed to register user '{username}' as new user due to an unknown reason", dto.Username);
            throw new F1ManagerUserException(UserErrorCode.UserRegistrationFailed, "Failed to register new user");
        }
        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto dto)
        {
            var user = await _repository.GetUserByUsername(dto.Username);
            if (user != null)
            {
                if (user.Password.Validate(dto.Password))
                {
                    if (!user.IsLockedOut)
                    {
                        return new UserLoginResponseDto
                        {
                            Success = true,
                            JwtToken = GenerateJwtTokenForUser(user.Id, user.IsAdministrator)
                        };
                    }

                    return new UserLoginResponseDto
                    {
                        Success = false,
                        ErrorMessage = user.LockoutReason
                    };
                }
            }

            return new UserLoginResponseDto
            {
                Success = false,
                ErrorMessage = "UserNotFound"
            };
        }

        private static UserDetailsDto ToUserDetailsDto(User domainModel)
        {
            return new UserDetailsDto
            {
                Id = domainModel.Id,
                DisplayName = domainModel.DisplayName,
                Username = domainModel.Username,
                EmailAddress = domainModel.EmailAddress,
                DateEmailVerified = domainModel.DateEmailVerified,
                RegisteredOn = domainModel.RegisteredOn,
                LastLoginOn = domainModel.LastLoginOn
            };
        }

        private  string GenerateJwtTokenForUser(Guid userId, bool isAdmin = false)
        {

            var mySecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_userOptions.Value.Secret));
            var signingCredentials = new SigningCredentials(mySecurityKey, SecurityAlgorithms.HmacSha256Signature);

            var tokenHandler = new JwtSecurityTokenHandler();

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                Issuer = _userOptions.Value.Issuer,
                Audience = _userOptions.Value.Audience,
                SigningCredentials = signingCredentials
            };
            if (isAdmin)
            {
                tokenDescriptor.Claims.Add("Admin", isAdmin.ToString());
            }

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        
        public UsersService(IUsersRepository repository,
            IUsersDomainService domainService,
            ILogger<UsersService> logger,
            IOptions<UsersOptions> userOptions)
        {
            _repository = repository;
            _domainService = domainService;
            _logger = logger;
            _userOptions = userOptions;
        }
    }
}
