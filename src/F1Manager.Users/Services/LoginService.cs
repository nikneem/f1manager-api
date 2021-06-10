using System;
using System.Collections.Generic;
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
            using var myRijndael = new RijndaelManaged();
            myRijndael.GenerateKey();
            myRijndael.GenerateIV();
            var key = Convert.ToBase64String(myRijndael.Key);
            var iv = Convert.ToBase64String(myRijndael.IV);

            return await _loginRepository.RegisterLoginAttempt(key, iv);
        }

        public LoginService(ILoginRepository loginRepository)
        {
            _loginRepository = loginRepository;
        }
    }
}
