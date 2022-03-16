param standardAppName string

@allowed([
  'functionapp'
  'linux'
  'app'
])
param kind string = 'app'

param sku object = {
  name: 'S1'
  capacity: 1
}

param location string = resourceGroup().location

var resourceName = '${standardAppName}-plan'

resource appFarm 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: resourceName
  location: location
  kind: kind
  sku: {
    name: sku.name
    capacity: sku.capacity
  }
}

output servicePlanName string = appFarm.name
output id string = appFarm.id
