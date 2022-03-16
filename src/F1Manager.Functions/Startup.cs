using F1Manager.Functions;
using HexMaster.Email.Configuration;
using Microsoft.Azure.Functions.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

[assembly: FunctionsStartup(typeof(Startup))]
namespace F1Manager.Functions;

public class Startup : FunctionsStartup
{
    public override void Configure(IFunctionsHostBuilder builder)
    {
        var serviceProvider = builder.Services.BuildServiceProvider();
        var configuration = serviceProvider.GetService<IConfiguration>();

        builder.Services.ConfigureEmailService(configuration.GetSection(EmailOptions.SectionName));
        builder.Services.AddApplicationInsightsTelemetry();
    }
}