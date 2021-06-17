using F1Manager.Shared.Base;

namespace F1Manager.Users.Exceptions
{
    public abstract class LoginErrorCode : ErrorCode
    {
        public static LoginErrorCode FailedToRegisterLoginAttempt;
        public static LoginErrorCode LoginAttemptFailedOrExpired;
        public static LoginErrorCode InvalidRefreshTokenOperation;
        public static LoginErrorCode InvalidRefreshToken;
        public static LoginErrorCode InactiveRefreshToken;
        public static LoginErrorCode RevokedRefreshToken;
        public static LoginErrorCode ExpiredRefreshToken;


        static LoginErrorCode()
        {
            FailedToRegisterLoginAttempt = new FailedToRegisterLoginAttempt();
            LoginAttemptFailedOrExpired = new LoginAttemptFailedOrExpired();
            InvalidRefreshTokenOperation = new InvalidRefreshTokenOperation();
            InvalidRefreshToken = new InvalidRefreshToken();
            InactiveRefreshToken = new InactiveRefreshToken();
            RevokedRefreshToken = new RevokedRefreshToken();
            ExpiredRefreshToken = new ExpiredRefreshToken();
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
    public class InvalidRefreshTokenOperation : LoginErrorCode
    {
        public override string Code => "Login.Errors.InvalidRefreshTokenOperation";
        public override string TranslationKey => "Login.Errors.InvalidRefreshTokenOperation";
    }
    public class InvalidRefreshToken : LoginErrorCode
    {
        public override string Code => "Login.Errors.InvalidRefreshToken";
        public override string TranslationKey => "Login.Errors.InvalidRefreshToken";
    }
    public class InactiveRefreshToken : LoginErrorCode
    {
        public override string Code => "Login.Errors.InactiveRefreshToken";
        public override string TranslationKey => "Login.Errors.InactiveRefreshToken";
    }
    public class RevokedRefreshToken : LoginErrorCode
    {
        public override string Code => "Login.Errors.RevokedRefreshToken";
        public override string TranslationKey => "Login.Errors.RevokedRefreshToken";
    }
    public class ExpiredRefreshToken : LoginErrorCode
    {
        public override string Code => "Login.Errors.ExpiredRefreshToken";
        public override string TranslationKey => "Login.Errors.ExpiredRefreshToken";
    }
}
