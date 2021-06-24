using System;
using System.Collections.Generic;
using System.Text;

namespace F1Manager.Admin.Configuration
{
    public class AdminOptions
    {
        public const string SectionName = "Administration";

        public string AzureStorageAccount { get; set; }
        public string CacheConnectionString { get; set; }
    }
}
