﻿using Microsoft.Extensions.Options;

namespace F1Manager.Users.Configuration
{
    public class UsersOptionsValidator : IValidateOptions<UsersOptions>
    {
        public ValidateOptionsResult Validate(string name, UsersOptions options)
        {
            if (options.AzureStorageAccount == default)
            {
                var optionName = $"{UsersOptions.SectionName}.{nameof(options.AzureStorageAccount)}";
                return ValidateOptionsResult.Fail($"Missing configuration setting for: {optionName}. The value may not be 0 or empty");
            }

            return ValidateOptionsResult.Success;
        }
    }
}
