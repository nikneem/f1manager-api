param standardAppName string

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

var resourceName = '${standardAppName}-plan'

resource appFarm 'Microsoft.Web/serverfarms@2020-12-01' = {
  name: resourceName
  location: resourceGroup().location
  kind: kind
  sku: {
    name: sku.name
    capacity: sku.capacity
  }
}

output servicePlanName string = appFarm.name
output id string = appFarm.id
