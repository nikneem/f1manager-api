﻿using System;
using System.Threading.Tasks;
using F1Manager.Users.DataTransferObjects;

namespace F1Manager.Users.Abstractions
{
    public interface ILoginService
    {

        Task<LoginAttemptDto> RequestLogin();
        Task<UserLoginResponseDto> Login(UserLoginRequestDto dto);

        Task<UserLoginResponseDto> Recover(Guid userId);
    }
}
