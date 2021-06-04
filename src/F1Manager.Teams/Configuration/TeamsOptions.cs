using System;
using System.Collections.Generic;
using System.Text;

namespace F1Manager.Teams.Configuration
{
  public  class TeamsOptions
    {
        public const string SectionName = "Teams";

        public string AzureStorageConnectionString { get; set; }
    }
}
