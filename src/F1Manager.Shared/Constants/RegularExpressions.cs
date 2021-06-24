using System.ComponentModel;

namespace F1Manager.Shared.Constants
{
    public class RegularExpressions
    {
        public const string EmailAddress = "^([a-zA-Z0-9_\\-\\.\\+]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})$";
        [Description("This is a regular expression to test passwords. At least 8 characters, one upper, one lower and one digit")]
        public const string UpperLowedDigit = "^(?=.*[a-z])(?=.*[A-Z])(?=.*\\d)[a-zA-Z\\d]{8,}$";
        public const string Username = "^(?=.{6,20}$)(?![_.])(?!.*[_.]{2})[a-zA-Z0-9._@]+(?<![_.])$";
        public const string Teamname = "^([\\w\\s\\d]){1,100}$";
        public const string UniqueResourceLocation = "(?<Protocol>\\w+):\\/\\/(?<Domain>[\\w@][\\w.:@]+)\\/?[\\w\\.?=%&=\\-@/$,]*";

    }
}
