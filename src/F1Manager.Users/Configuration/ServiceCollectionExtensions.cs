using F1Manager.Shared.ServiceCollectionExtensions;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Repositories;
using F1Manager.Users.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace F1Manager.Users.Configuration
{
    public static class ServiceCollectionExtensions
    {
        public static void ConfigureUsers(this IServiceCollection serviceCollection)
        {

            var configuration = serviceCollection
                .BuildServiceProvider()
                .GetService<IConfiguration>();

            serviceCollection.ConfigureAndValidate<UsersOptions, UsersOptionsValidator>(configuration);

            serviceCollection.AddScoped<IUsersService, UsersService>();
            serviceCollection.AddScoped<ILoginsService, LoginsService>();
            serviceCollection.AddTransient<IUsersDomainService, UsersDomainService>();

            serviceCollection.AddScoped<IUsersRepository, UsersRepository>();
            serviceCollection.AddScoped<ILoginsRepository, LoginsRepository>();
            serviceCollection.AddScoped<IRefreshTokensRepository, RefreshTokensRepository>();
        }
    }
}