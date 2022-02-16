using F1Manager.Leagues.Abstractions;
using F1Manager.Leagues.Repository;
using F1Manager.Leagues.Services;
using F1Manager.Shared.ServiceCollectionExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace F1Manager.Leagues.Configuration
{
    public static class ServiceCollectionExtensions
    {

        public static void ConfigureLeagues(this IServiceCollection serviceCollection)
        {

            var configuration = serviceCollection
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            serviceCollection.ConfigureAndValidate<LeaguesOptions, LeaguesOptionsValidator>(configuration);

            serviceCollection.AddScoped<ILeaguesService, LeaguesService>();

            serviceCollection.AddScoped<ILeaguesRepository, LeaguesRepository>();
            serviceCollection.AddScoped<ILeagueInvitationsRepository, LeagueInvitationsRepository>();
            serviceCollection.AddScoped<ILeagueRequestsRepository, LeagueRequestsRepository>();

        }
    }
}