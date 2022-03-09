using F1Manager.Email.Abstractions;
using F1Manager.Email.Services;
using F1Manager.Shared.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace F1Manager.Email.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static void ConfigureMailDispatcher(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            serviceCollection.ConfigureAndValidate<MailDispatcherOptions, MailDispatcherOptionsValidator>(configuration);

            serviceCollection.AddScoped<IMailDispatcher, MailDispatcher>();
        }
    }
}