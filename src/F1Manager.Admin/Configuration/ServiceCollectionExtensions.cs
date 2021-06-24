using F1Manager.Admin.Drivers.Abstractions;
using F1Manager.Admin.Drivers.Repositories;
using F1Manager.Admin.Drivers.Services;
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

            serviceCollection.AddScoped<IDriversService, DriversService>();

            serviceCollection.AddScoped<IDriversRepository, DriversRepository>();

        }

    }
}
