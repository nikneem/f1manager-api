using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;


namespace F1Manager.Shared.ServiceCollectionExtensions
{
    public static class ServiceCollectionExtensions
    {

        public static IServiceCollection ConfigureAndValidate<T, TValidator>(this IServiceCollection services, IConfiguration configurationSection)
            where T : class, new()
            where TValidator : class, IValidateOptions<T>, new()
        {
            services.Configure<T>(configurationSection);
            services.AddSingleton<IValidateOptions<T>, TValidator>();

            var container = services.BuildServiceProvider();

            try
            {
                var options = container.GetService<IOptions<T>>();

                var validator = new TValidator();
                var validationResult = validator.Validate(string.Empty, options.Value);
                if (validationResult.Failed)
                {
                    var message = $"AppSettings section '{typeof(T).Name}' failed validation. Reason: {validationResult.FailureMessage}";

                    throw new OptionsValidationException(string.Empty, typeof(T), new[] { message });
                }
            }

            finally
            {
                container.Dispose();
            }

            return services;
        }

    }
}
