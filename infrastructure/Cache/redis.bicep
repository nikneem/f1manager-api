param systemName string = 'f1man'
@allowed([
  'dev'
  'test'
  'acc'
  'prod'
])
param environmentName string = 'prod'
param azureRegion string = 'weu'

var cacheName = '${systemName}-${environmentName}-${azureRegion}-cache'

resource redisCache 'Microsoft.Cache/redis@2020-12-01' = {
  location: resourceGroup().location
  name: cacheName
  properties: {
    sku: {
      name: 'Basic'
      family: 'C'
      capacity: 0
    }
  }
}
