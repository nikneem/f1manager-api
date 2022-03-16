using Microsoft.Extensions.Options;

namespace F1Manager.Email.Configuration
{
    public class MailDispatcherOptionsValidator : IValidateOptions<MailDispatcherOptions>
    {
        public ValidateOptionsResult Validate(string name, MailDispatcherOptions options)
        {
            if (options.AzureStorageAccount == default)
            {
                var optionName = $"{nameof(options.AzureStorageAccount)}";
                return ValidateOptionsResult.Fail($"Missing configuration setting for: {optionName}. The value may not be 0 or empty");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
