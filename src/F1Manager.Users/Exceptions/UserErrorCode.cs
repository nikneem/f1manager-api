using F1Manager.Shared.Base;

namespace F1Manager.Users.Exceptions;

public abstract class UserErrorCode : ErrorCode
{
    public static readonly UserErrorCode DisplayNameNullOrEmpty = new DisplayNameNullOrEmpty();
    public static readonly UserErrorCode UsernameNullOrEmpty = new UsernameNullOrEmpty();
    public static readonly UserErrorCode InvalidUsername = new InvalidUsername();
    public static readonly UserErrorCode UsernameNotUnique = new InvalidUsername();
    public static readonly UserErrorCode EmailNotUnique = new EmailNotUnique();
    public static readonly UserErrorCode PasswordNullOrEmpty = new PasswordNullOrEmpty();
    public static readonly UserErrorCode InvalidPassword = new InvalidPassword();
    public static readonly UserErrorCode EmailNullOrEmpty = new EmailNullOrEmpty();
    public static readonly UserErrorCode InvalidEmail = new InvalidEmail();
    public static readonly UserErrorCode UserRegistrationFailed = new UserRegistrationFailed();
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
public class EmailNotUnique : UserErrorCode
{
    public override string Code => "Users.Errors.EmailNotUnique";
    public override string TranslationKey => "Users.Errors.EmailNotUnique";
}