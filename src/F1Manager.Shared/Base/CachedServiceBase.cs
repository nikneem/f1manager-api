using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace F1Manager.Shared.Base
{
    public class CachedServiceBase<TDerivedClass>
    {

        private readonly ILogger<TDerivedClass> _logger;
        private readonly string _connectionString;

        private IConnectionMultiplexer _connection;
        private IDatabase _database;
        private bool _cacheIsAvailable;


        protected async Task<T> GetFromCache<T>(string cacheKey, Func<Task<T>> initializeFunction)
        {
            if (!_cacheIsAvailable)
            {
                await Connect();
            }

            try
            {
                var redisValue= await _database.StringGetAsync(cacheKey);
                if (redisValue.HasValue)
                {
                    return JsonConvert.DeserializeObject<T>(redisValue.ToString());
                }
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to retrieve value from cache", ex);
                _cacheIsAvailable = false;
            }

            var initializedObject = await initializeFunction();
            if (initializedObject != null)
            {
                var jsonValue = JsonConvert.SerializeObject(initializedObject);
                if (_cacheIsAvailable)
                {
                    await _database.StringSetAsync(cacheKey, jsonValue);
                }
            }

            return initializedObject;
        }

        protected async Task<bool> InvalidateCache(string cacheKey)
        {
            if (!_cacheIsAvailable)
            {
                await Connect();
            }

            try
            {
                return await _database.KeyDeleteAsync(cacheKey);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to remove value from cache", ex);
                _cacheIsAvailable = false;
            }

            return false;
        }

        private async Task Connect()
        {
            try
            {
                _connection = await ConnectionMultiplexer.ConnectAsync(_connectionString);
                _database = _connection.GetDatabase();
                _cacheIsAvailable = true;
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to connect to redis cache instance", ex);
                _cacheIsAvailable = false;
            }
        }

        protected CachedServiceBase(string redisConnectionString,ILogger<TDerivedClass> logger)
        {
            _connectionString = redisConnectionString;
            _logger = logger;
            if (string.IsNullOrWhiteSpace(_connectionString))
            {
                _logger.LogCritical("The connection string to Azure Cache for Redis is not configured properly, cache will not function at this time");
            }
        }
    }
}
