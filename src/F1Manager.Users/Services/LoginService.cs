using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Configuration;
using F1Manager.Users.DataTransferObjects;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace F1Manager.Users.Services
{
public sealed     class LoginService: ILoginService
    {
        private readonly ILoginRepository _loginRepository;
        private readonly IUsersRepository _usersRepository;
        private readonly IOptions<UsersOptions> _userOptions;

        public async Task<LoginAttemptDto> RequestLogin()
        {
            using var rijndael = new RijndaelManaged();
            rijndael.GenerateKey();
            rijndael.GenerateIV();
            var key = Convert.ToBase64String(rijndael.Key);
            var iv = Convert.ToBase64String(rijndael.IV);

            return await _loginRepository.RegisterLoginAttempt(key, iv);
        }


        public async Task<UserLoginResponseDto> Login(UserLoginRequestDto dto)
        {
            dto = await ValidateLogin(dto);
            var user = await _usersRepository.GetByUsername(dto.Username);
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

        public async Task<UserLoginResponseDto> Recover(Guid userId)
        {
            var user = await _usersRepository.GetById(userId);
            if (user != null)
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
                ErrorMessage = "UserNotFound"
            };
        }

        private async Task<UserLoginRequestDto> ValidateLogin(UserLoginRequestDto dto)
        {
            var attempt = await _loginRepository.ValidateLoginAttempt(dto.Id);
            var cipher = Convert.FromBase64String(dto.Password);
            var key = Convert.FromBase64String(attempt.Key);
            var iv = Convert.FromBase64String(attempt.Vector);
            dto.Password = DecryptStringFromBytes(cipher, key, iv);
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


        static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            using var rijAlg = new RijndaelManaged {Key = key, IV = iv};

            var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);
            using var msDecrypt = new MemoryStream(cipherText);
            using var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read);
            using var srDecrypt = new StreamReader(csDecrypt);
            return srDecrypt.ReadToEnd();
        }


        public LoginService(ILoginRepository loginRepository,
            IUsersRepository usersRepository, 
            IOptions<UsersOptions> userOptions)
        {
            _loginRepository = loginRepository;
            _usersRepository = usersRepository;
            _userOptions = userOptions;
        }
    }
}
