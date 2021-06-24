using Microsoft.Extensions.Options;

namespace F1Manager.Admin.Configuration
{
    public class AdminOptionsValidator : IValidateOptions<AdminOptions>
    {
        public ValidateOptionsResult Validate(string name, AdminOptions options)
        {
            if (options.AzureStorageAccount == default)
            {
                var optionName = $"{AdminOptions.SectionName}.{nameof(options.AzureStorageAccount)}";
                return ValidateOptionsResult.Fail($"Missing configuration setting for: {optionName}. The value may not be 0 or empty");
            }

            if (options.CacheConnectionString == default)
            {
                var optionName = $"{AdminOptions.SectionName}.{nameof(options.CacheConnectionString)}";
                return ValidateOptionsResult.Fail($"Missing configuration setting for: {optionName}. The value may not be 0 or empty");
            }


            return ValidateOptionsResult.Success;
        }
    }
}
