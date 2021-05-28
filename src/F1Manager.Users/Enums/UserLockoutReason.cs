namespace F1Manager.Users.Enums
{
    public abstract class UserLockoutReason
    {

        public static UserLockoutReason EmailNotVerified;
        public static UserLockoutReason SystemAbuse;
        public static UserLockoutReason UnknownLoginOrigin;
        public static UserLockoutReason[] All;

        public abstract string Key { get; }
        public abstract string TranslationKey { get; }


        static UserLockoutReason()
        {
            All = new[]
            {
                EmailNotVerified = new EmailNotVerified(),
                SystemAbuse = new SystemAbuse(),
                UnknownLoginOrigin = new UnknownLoginOrigin()
            };
        }

    }

    public class EmailNotVerified : UserLockoutReason
    {
        public override string Key => "EmailNotVerified";
        public override string TranslationKey => "System.LockoutReason.EmailNotVerified";
    }
    public class SystemAbuse : UserLockoutReason
    {
        public override string Key => "SystemAbuse";
        public override string TranslationKey => "System.LockoutReason.SystemAbuse";
    }
    public class UnknownLoginOrigin : UserLockoutReason
    {
        public override string Key => "UnknownLoginOrigin";
        public override string TranslationKey => "System.LockoutReason.UnknownLoginOrigin";
    }

}
