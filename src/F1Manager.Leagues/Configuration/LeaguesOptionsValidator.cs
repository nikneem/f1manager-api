using Microsoft.Extensions.Options;

namespace F1Manager.Leagues.Configuration
{
    public class LeaguesOptionsValidator : IValidateOptions<LeaguesOptions>
    {
        public ValidateOptionsResult Validate(string name, LeaguesOptions options)
        {
            if (options.AzureStorageAccount == default)
            {
                var optionName = $"{nameof(options.AzureStorageAccount)}";
                return ValidateOptionsResult.Fail($"Missing configuration setting for: {optionName}. The value may not be 0 or empty");
            }

            if (options.CacheConnectionString == default)
            {
                var optionName = $"{nameof(options.CacheConnectionString)}";
                return ValidateOptionsResult.Fail($"Missing configuration setting for: {optionName}. The value may not be 0 or empty");
            }


            return ValidateOptionsResult.Success;
        }
    }
}
