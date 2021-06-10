namespace F1Manager.Users.Configuration
{
    public class UsersOptions
    {
        public const string SectionName = "Users";

        public string AzureStorageAccount { get; set; }
        public string Secret { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}

