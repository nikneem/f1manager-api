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
  'standard'
  'premium'
])
param sku string = 'standard'

var keyVaultName = '${systemName}-${environmentName}-${azureRegion}-kv'

resource keyVault 'Microsoft.KeyVault/vaults@2021-04-01-preview' = {
  name: keyVaultName
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
