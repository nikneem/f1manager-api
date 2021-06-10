using System;
using System.Collections.Generic;
using System.Text;
using F1Manager.Shared.Base;

namespace F1Manager.Users.Exceptions
{
    public abstract class LoginErrorCode : ErrorCode
    {
        public static LoginErrorCode FailedToRegisterLoginAttempt;
        public static LoginErrorCode LoginAttemptFailedOrExpired;
        

        static LoginErrorCode()
        {
            FailedToRegisterLoginAttempt = new FailedToRegisterLoginAttempt();
            LoginAttemptFailedOrExpired = new LoginAttemptFailedOrExpired();
        }
    }


    public class FailedToRegisterLoginAttempt : LoginErrorCode
    {
        public override string Code => "Login.Errors.FailedToRegisterLoginAttempt";
        public override string TranslationKey => "Login.Errors.FailedToRegisterLoginAttempt";
    }
    public class LoginAttemptFailedOrExpired : LoginErrorCode
    {
        public override string Code => "Login.Errors.LoginAttemptFailedOrExpired";
        public override string TranslationKey => "Login.Errors.LoginAttemptFailedOrExpired";
    }
}
