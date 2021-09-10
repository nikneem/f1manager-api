param standardAppName string

var resourceName = '${standardAppName}-cache'

resource redisCache 'Microsoft.Cache/redis@2020-12-01' = {
  location: resourceGroup().location
  name: resourceName
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
  }
}

output primaryKey string = listKeys(redisCache.id, redisCache.apiVersion).primaryKey
output secret object = {
  name: 'CacheConnectionString'
  value: '${resourceName}.redis.cache.windows.net:6380,password=${listKeys(redisCache.id, redisCache.apiVersion).primaryKey},ssl=True,abortConnect=False'
}
