using F1Manager.Shared.Base;

namespace F1Manager.Users.Exceptions
{

    public abstract class UserErrorCode : ErrorCode
    {
        public static UserErrorCode DisplayNameNullOrEmpty;
        public static UserErrorCode UsernameNullOrEmpty;
        public static UserErrorCode InvalidUsername;
        public static UserErrorCode UsernameNotUnique;
        public static UserErrorCode PasswordNullOrEmpty;
        public static UserErrorCode InvalidPassword;
        public static UserErrorCode EmailNullOrEmpty;
        public static UserErrorCode InvalidEmail;
        public static UserErrorCode UserRegistrationFailed;

        static UserErrorCode()
        {
            DisplayNameNullOrEmpty = new DisplayNameNullOrEmpty();
            UsernameNullOrEmpty = new UsernameNullOrEmpty();
            InvalidUsername = new InvalidUsername();
            UsernameNotUnique = new InvalidUsername();
            PasswordNullOrEmpty = new PasswordNullOrEmpty();
            InvalidPassword = new InvalidPassword();
            EmailNullOrEmpty = new EmailNullOrEmpty();
            InvalidEmail = new InvalidEmail();
            UserRegistrationFailed = new UserRegistrationFailed();
        }
    }


    public class DisplayNameNullOrEmpty : UserErrorCode
    {
        public override string Code => "Users.Errors.DisplayNameNullOrEmpty";
        public override string TranslationKey => "Users.Errors.DisplayNameNullOrEmpty";
    }
    public class UsernameNullOrEmpty : UserErrorCode
    {
        public override string Code => "Users.Errors.UsernameNullOrEmpty";
        public override string TranslationKey => "Users.Errors.UsernameNullOrEmpty";
    }
    public class InvalidUsername : UserErrorCode
    {
        public override string Code => "Users.Errors.InvalidUsername";
        public override string TranslationKey => "Users.Errors.InvalidUsername";
    }
    public class UsernameNotUnique : UserErrorCode
    {
        public override string Code => "Users.Errors.UsernameNotUnique";
        public override string TranslationKey => "Users.Errors.UsernameNotUnique";
    }
    public class InvalidPassword : UserErrorCode
    {
        public override string Code => "Users.Errors.InvalidPassword";
        public override string TranslationKey => "Users.Errors.InvalidPassword";
    }
    public class PasswordNullOrEmpty : UserErrorCode
    {
        public override string Code => "Users.Errors.PasswordNullOrEmpty";
        public override string TranslationKey => "Users.Errors.PasswordNullOrEmpty";
    }
    public class InvalidEmail : UserErrorCode
    {
        public override string Code => "Users.Errors.InvalidEmail";
        public override string TranslationKey => "Users.Errors.InvalidEmail";
    }
    public class EmailNullOrEmpty : UserErrorCode
    {
        public override string Code => "Users.Errors.EmailNullOrEmpty";
        public override string TranslationKey => "Users.Errors.EmailNullOrEmpty";
    }
    public class UserRegistrationFailed : UserErrorCode
    {
        public override string Code => "Users.Errors.UserRegistrationFailed";
        public override string TranslationKey => "Users.Errors.UserRegistrationFailed";
    }
}
