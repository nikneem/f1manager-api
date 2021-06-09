param systemName string = 'f1man'
@allowed([
  'dev'
  'test'
  'acc'
  'prod'
])
param environmentName string = 'prod'
param azureRegion string = 'weu'

param developerObjectIds array = [
  'ce00c98d-c389-47b0-890e-7f156f136ebd'
  '4511674d-9538-4852-a05d-4416c894aeb8'
  'ad7deb2e-6a44-4309-a037-6f0ff6b273b2'
  'f2256599-cbea-44ae-aa54-2daafaac605a'
]

module redisCacheModule 'Cache/redis.bicep' = {
  name: 'redisCacheModule'
  params: {
    systemName: systemName
    environmentName: environmentName
    azureRegion: azureRegion
  }
}
module storageAccountModule 'Storage/storageAccounts.bicep' = {
  name: 'storageAccountModule'
  params: {
    systemName: systemName
    environmentName: environmentName
    azureRegion: azureRegion
  }
}
module applicationInsightsModule 'Insights/components.bicep' = {
  name: 'applicationInsightsDeploy'
  params: {
    systemName: systemName
    environmentName: environmentName
    azureRegion: azureRegion
  }
}
module keyVaultModule 'KeyVault/vaults.bicep' = {
  name: 'keyVaultDeploy'
  params: {
    systemName: systemName
    environmentName: environmentName
    azureRegion: azureRegion
  }
}
module sqlServerModule 'Sql/servers.bicep' = {
  name: 'sqlServerModule'
  params: {
    systemName: systemName
    environmentName: environmentName
    azureRegion: azureRegion
  }
}
module sqlServerDatabaseModule 'Sql/servers/database.bicep' = {
  dependsOn: [
    sqlServerModule
  ]
  name: 'sqlServerDatabaseModule'
  params: {
    databaseName: sqlServerModule.outputs.databaseName
    sqlServerName: sqlServerModule.outputs.sqlServerName
  }
}
module serviceBusListenerSecret 'KeyVault/vaults/secrets.bicep' = {
  dependsOn: [
    keyVaultModule
    storageAccountModule
    sqlServerModule
  ]
  name: 'serviceBusListenerSecretDeploy'
  params: {
    keyVault: keyVaultModule.outputs.keyVaultName
    secrets: union(storageAccountModule.outputs.secret, sqlServerModule.outputs.secret)
  }
}
module appServicePlanModule 'Web/serverfarms.bicep' = {
  name: 'appServicePlanDeploy'
  params: {
    systemName: systemName
    environmentName: environmentName
    azureRegion: azureRegion
  }
}
resource webAppModule 'Microsoft.Web/sites@2020-12-01' = {
  dependsOn: [
    appServicePlanModule
    applicationInsightsModule
    storageAccountModule
  ]
  name: '${systemName}-${environmentName}-${azureRegion}-app'
  location: resourceGroup().location
  kind: 'functionapp'
  properties: {
    serverFarmId: appServicePlanModule.outputs.id
    httpsOnly: true
    clientAffinityEnabled: false
  }
  identity: {
    type: 'SystemAssigned'
  }
}
module keyVaultAccessPolicyModule 'KeyVault/vaults/accessPolicies.bicep' = {
  name: 'keyVaultAccessPolicyDeploy'
  dependsOn: [
    keyVaultModule
    webAppModule
  ]
  params: {
    keyVaultName: keyVaultModule.outputs.keyVaultName
    principalId: webAppModule.identity.principalId
  }
}
resource config 'Microsoft.Web/sites/config@2020-12-01' = {
  dependsOn: [
    keyVaultAccessPolicyModule
    webAppModule
    serviceBusListenerSecret
  ]
  name: '${webAppModule.name}/web'
  properties: {
    appSettings: [
      {
        name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
        value: applicationInsightsModule.outputs.instrumentationKey
      }
      {
        name: 'AzureWebJobsStorage'
        value: '@Microsoft.KeyVault(SecretUri=${keyVaultModule.outputs.keyVaultUrl}/secrets/${storageAccountModule.outputs.secretName})'
      }
      {
        name: 'FUNCTIONS_EXTENSION_VERSION'
        value: '~3'
      }
      {
        name: 'FUNCTIONS_WORKER_RUNTIME'
        value: 'dotnet'
      }
      {
        name: 'WEBSITE_CONTENTAZUREFILECONNECTIONSTRING'
        value: '@Microsoft.KeyVault(SecretUri=${keyVaultModule.outputs.keyVaultUrl}/secrets/${storageAccountModule.outputs.secretName})'
      }
      {
        name: 'ComponentImagesStorageAccount'
        value: '@Microsoft.KeyVault(SecretUri=${keyVaultModule.outputs.keyVaultUrl}/secrets/${storageAccountModule.outputs.secretName})'
      }
      {
        name: 'WEBSITE_CONTENTSHARE'
        value: 'azure-function'
      }
    ]
  }
}

@batchSize(1)
module developerAccessPolicies 'KeyVault/vaults/accessPolicies.bicep' = [for developer in developerObjectIds: {
  name: 'developer${developer}'
  dependsOn: [
    keyVaultModule
    keyVaultAccessPolicyModule
  ]
  params: {
    keyVaultName: keyVaultModule.outputs.keyVaultName
    principalId: developer
  }
}]
