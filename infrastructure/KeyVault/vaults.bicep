param standardAppName string

@allowed([
  'standard'
  'premium'
])
param sku string = 'standard'

var resourceName = '${standardAppName}-kv'

resource keyVault 'Microsoft.KeyVault/vaults@2021-04-01-preview' = {
  name: resourceName
  location: resourceGroup().location
  properties: {
    tenantId: subscription().tenantId
    sku: {
      family: 'A'
      name: sku
    }
    accessPolicies: []
    enableSoftDelete: false
  }
}

output keyVaultName string = keyVault.name
output keyVaultUrl string = keyVault.properties.vaultUri
