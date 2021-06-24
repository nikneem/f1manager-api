using F1Manager.Shared.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace F1Manager.Admin.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static void ConfigureAdministration(this IServiceCollection serviceCollection)
        {

            var configuration = serviceCollection
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            serviceCollection.ConfigureAndValidate<AdminOptions, AdminOptionsValidator>(
                configuration.GetSection(AdminOptions.SectionName));

        }

    }
}
