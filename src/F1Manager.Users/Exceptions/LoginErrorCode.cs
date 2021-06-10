using System;
using System.Collections.Generic;
using System.Text;
using F1Manager.Shared.Base;

namespace F1Manager.Users.Exceptions
{
    public abstract class LoginErrorCode : ErrorCode
    {
        public static LoginErrorCode FailedToRegisterLoginAttempt;

        static LoginErrorCode()
        {
            FailedToRegisterLoginAttempt = new FailedToRegisterLoginAttempt();
        }
    }


    public class FailedToRegisterLoginAttempt : LoginErrorCode
    {
        public override string Code => "Login.Errors.FailedToRegisterLoginAttempt";
        public override string TranslationKey => "Login.Errors.FailedToRegisterLoginAttempt";
    }
}
