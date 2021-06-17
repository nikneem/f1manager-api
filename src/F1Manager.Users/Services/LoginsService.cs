using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.DataTransferObjects;
using F1Manager.Users.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace F1Manager.Users.Services
{
    public sealed class LoginsService : ILoginsService
    {
        private readonly ILoginsRepository _loginsRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IRefreshTokensRepository _refreshTokensRepository;
        private readonly IOptions<UsersOptions> _userOptions;

        public async Task<LoginAttemptDto> RequestLogin()
        {
            var attempt = new LoginAttempt();
            if (await _loginsRepository.RegisterLoginAttempt(attempt))
            {
                return new LoginAttemptDto
                {
                    Id = attempt.Id,
                    RsaPublicKey = attempt.RsaPublicKey
                };
            }

            return null;
        }
        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto dto, IPAddress ipAddress)
        {
            dto = await ValidateLogin(dto);
            var user = await _usersRepository.GetByUsername(dto.Username);
            if (user != null)
            {
                if (user.Password.Validate(dto.Password))
                {
                    return await Generate(user, ipAddress) ??
                           new UserLoginResponseDto
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
        public async Task<UserLoginResponseDto> Refresh(string token, IPAddress ipAddress)
        {
            var userId = await _refreshTokensRepository.ValidateRefreshToken(token);
            var user = await _usersRepository.GetById(userId);
            return await Generate(user, ipAddress) ??
                   new UserLoginResponseDto
                   {
                       Success = false,
                       ErrorMessage = user.LockoutReason
                   };
        }
        private async Task<UserLoginResponseDto> Generate(User user, IPAddress ipAddress)
        {
            if (!user.IsLockedOut)
            {
                return new UserLoginResponseDto
                {
                    Success = true,
                    JwtToken = GenerateJwtTokenForUser(user.Id, user.IsAdministrator),
                    RefreshToken = await _refreshTokensRepository.GenerateRefreshToken(user.Id, ipAddress.ToString())
                };
            }

            return null;
        }

        private async Task<UserLoginRequestDto> ValidateLogin(UserLoginRequestDto dto)
        {
            var attempt = await _loginsRepository.ValidateLoginAttempt(dto.Id);
            dto = attempt.DecryptUsernameAndPassword(dto);
            return dto;
        }

        private string GenerateJwtTokenForUser(Guid userId, bool isAdmin = false)
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
                Expires = DateTime.UtcNow.AddMinutes(15),
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


        static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            using var rijAlg = new RijndaelManaged {Key = key, IV = iv};

            var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }


        public LoginsService(
            ILoginsRepository loginsRepository,
            IUsersRepository usersRepository,
            IRefreshTokensRepository refreshTokensRepository,
            IOptions<UsersOptions> userOptions)
        {
            _loginsRepository = loginsRepository;
            _usersRepository = usersRepository;
            _refreshTokensRepository = refreshTokensRepository;
            _userOptions = userOptions;
        }
    }
}