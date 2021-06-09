param systemName string = 'f1man'
@allowed([
  'dev'
  'test'
  'acc'
  'prod'
])
param environmentName string = 'dev'
param azureRegion string = 'weu'

param developerObjectIds array = [
  'de55357b-c155-4de7-916f-ff12755cf5fb'
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
module keyVaultSecrets 'KeyVault/vaults/secrets.bicep' = {
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
module appServicePlanModule 'Web/serverFarms.bicep' = {
  name: 'appServicePlanDeploy'
  params: {
    systemName: systemName
    environmentName: environmentName
    azureRegion: azureRegion
  }
}

module webAppModule 'Web/sites.bicep' = {
  dependsOn: [
    appServicePlanModule
  ]
  name: 'webAppModule'
  params: {
    systemName: systemName
    environmentName: environmentName
    azureRegion: azureRegion
    appServicePlanId: appServicePlanModule.outputs.id
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
    principalId: webAppModule.outputs.servicePrincipal
  }
}

resource config 'Microsoft.Web/sites/config@2020-12-01' = {
  dependsOn: [
    keyVaultAccessPolicyModule
    webAppModule
    keyVaultSecrets
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
