param sqlServerName string
param databaseName string

param sku object = {
  name: 'Standard'
  capacity: 10
}

resource database 'Microsoft.Sql/servers/databases@2021-02-01-preview' = {
  location: resourceGroup().location
  name: '${sqlServerName}/${databaseName}'
  sku: {
    name: sku.name
    capacity: sku.capacity
  }
  properties: {}
}
