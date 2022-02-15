using F1Manager.Admin.Abstractions;
using F1Manager.Admin.Chassises.Abstractions;
using F1Manager.Admin.Chassises.Repositories;
using F1Manager.Admin.Chassises.Services;
using F1Manager.Admin.Drivers.Abstractions;
using F1Manager.Admin.Drivers.Repositories;
using F1Manager.Admin.Drivers.Services;
using F1Manager.Admin.Engines.Abstractions;
using F1Manager.Admin.Engines.Repositories;
using F1Manager.Admin.Engines.Services;
using F1Manager.Admin.Services;
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

            serviceCollection.ConfigureAndValidate<AdminOptions, AdminOptionsValidator>(configuration);

            serviceCollection.AddScoped<IUploadService, UploadService>();
            serviceCollection.AddScoped<IDriversService, DriversService>();
            serviceCollection.AddScoped<IEnginesService, EnginesService>();
            serviceCollection.AddScoped<IChassisesService, ChassisesService>();

            serviceCollection.AddScoped<IDriversRepository, DriversRepository>();
            serviceCollection.AddScoped<IEnginesRepository, EnginesRepository>();
            serviceCollection.AddScoped<IChassisesRepository, ChassisesRepository>();

        }

    }
}
