namespace F1Manager.Teams.Configuration
{
    public class TeamsOptions
    {
        public const string SectionName = "Teams";

        public string AzureStorageConnectionString { get; set; }
        public string RedisCacheConnectionString { get; set; }
    }
}