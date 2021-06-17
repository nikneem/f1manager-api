using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using F1Manager.Teams.Configuration;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace F1Manager.Teams.Health
{
    public class RedisCacheHealthCheck : IHealthCheck
    {
        private readonly string _redisCacheConnectionString;

        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = new CancellationToken())
        {
            try
            {
                var connection = await ConnectionMultiplexer.ConnectAsync(_redisCacheConnectionString);
                var database = connection.GetDatabase();
            }
            catch (Exception)
            {
                return HealthCheckResult.Degraded();
            }
            return HealthCheckResult.Healthy();
        }

        public RedisCacheHealthCheck(IOptions<TeamsOptions> options)
        {
            _redisCacheConnectionString = options.Value.CacheConnectionString;
        }

    }
}
