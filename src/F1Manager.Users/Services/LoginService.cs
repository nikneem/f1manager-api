using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using F1Manager.Users.Abstractions;
using F1Manager.Users.DataTransferObjects;

namespace F1Manager.Users.Services
{
public sealed     class LoginService: ILoginService
    {
        private readonly ILoginRepository _loginRepository;

        public async Task<LoginAttemptDto> RequestLogin()
        {
            using var rijndael = new RijndaelManaged();
            rijndael.GenerateKey();
            rijndael.GenerateIV();
            var key = Convert.ToBase64String(rijndael.Key);
            var iv = Convert.ToBase64String(rijndael.IV);

            return await _loginRepository.RegisterLoginAttempt(key, iv);
        }
        public async Task<UserLoginRequestDto> ValidateLogin(UserLoginRequestDto dto)
        {
            var attempt = await _loginRepository.ValidateLoginAttempt(dto.LoginId);
            var cipher = Convert.FromBase64String(dto.Password);
            var key = Convert.FromBase64String(attempt.Key);
            var iv = Convert.FromBase64String(attempt.Vector);
            dto.Password = DecryptStringFromBytes(cipher, key, iv);
            return dto;
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


        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
    }
}
