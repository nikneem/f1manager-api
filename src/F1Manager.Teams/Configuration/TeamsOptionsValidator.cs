using Microsoft.Extensions.Options;

namespace F1Manager.Teams.Configuration
{
    public class TeamsOptionsValidator : IValidateOptions<TeamsOptions>
    {
        public ValidateOptionsResult Validate(string name, TeamsOptions options)
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
