using F1Manager.Shared.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace F1Manager.Teams.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static void ConfigureTeams(this IServiceCollection serviceCollection)
        {

            var configuration = serviceCollection
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            serviceCollection.ConfigureAndValidate<TeamsOptions, TeamsOptionsValidator>(
                configuration.GetSection(TeamsOptions.SectionName));

        }
    }
}