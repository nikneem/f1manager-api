param systemName string = 'f1man'
@allowed([
  'dev'
  'test'
  'acc'
  'prod'
])
param environmentName string = 'prod'
param azureRegion string = 'weu'

@allowed([
  'functionapp'
  'linux'
  'app'
])
param kind string = 'app'

param sku object = {
  name: 'B1'
  capacity: 1
}

var servicePlanName = toLower('${systemName}-${environmentName}-${azureRegion}-plan')

resource appFarm 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: servicePlanName
  location: resourceGroup().location
  kind: kind
  sku: {
    name: sku.name
    capacity: sku.capacity
  }
}

output servicePlanName string = servicePlanName
output id string = appFarm.id
