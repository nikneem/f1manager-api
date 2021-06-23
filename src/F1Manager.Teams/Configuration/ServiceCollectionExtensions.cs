using F1Manager.Shared.ServiceCollectionExtensions;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.Repositories;
using F1Manager.Teams.Services;
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

            serviceCollection.AddScoped<ITeamsService, TeamsService>();
            serviceCollection.AddScoped<ITeamsDomainService, TeamsDomainService>();

            serviceCollection.AddScoped<ITeamsRepository, TeamsRepository>();
            serviceCollection.AddScoped<IDriversReadRepository, DriversReadRepository>();
            serviceCollection.AddScoped<IEnginesReadRespository, EnginesReadRespository>();
            serviceCollection.AddScoped<IChassisReadRespository, ChassisReadRespository>();

        }
    }
}