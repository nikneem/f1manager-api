namespace F1Manager.Users.Configuration
{
    public class UsersOptions
    {
        public const string SectionName = "Users";

        public string AzureStorageAccount { get; set; }
        public string CacheConnectionString { get; set; }
    }
}

