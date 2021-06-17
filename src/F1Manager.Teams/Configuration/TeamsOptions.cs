namespace F1Manager.Teams.Configuration
{
    public class TeamsOptions
    {
        public const string SectionName = "Teams";

        public string AzureStorageAccount { get; set; }
        public string CacheConnectionString { get; set; }
    }
}