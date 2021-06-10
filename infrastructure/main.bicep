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
@secure()
param jwtSignatureSecret string = newGuid()

var webAppName = '${systemName}-${environmentName}-${azureRegion}-app'
var tables = [
  'Users'
  'Logins'
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
module storageAccountTables 'Storage/tableServices/tables.bicep' = {
  dependsOn: [
    storageAccountModule
  ]
  name: 'storageAccountTables'
  params: {
    storageAccountName: storageAccountModule.outputs.storageAccountName
    tableNames: tables
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
    redisCacheModule
  ]
  name: 'keyVaultSecrets'
  params: {
    keyVault: keyVaultModule.outputs.keyVaultName
    secrets: union(storageAccountModule.outputs.secret, sqlServerModule.outputs.secret, redisCacheModule.outputs.secret)
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
    webAppName: webAppName
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
    redisCacheModule
  ]
  name: '${webAppName}/web'
  properties: {
    appSettings: [
      {
        name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
        value: applicationInsightsModule.outputs.instrumentationKey
      }
      {
        name: 'Users:AzureStorageAccount'
        value: '@Microsoft.KeyVault(SecretUri=${keyVaultModule.outputs.keyVaultUrl}/secrets/${storageAccountModule.outputs.secretName})'
      }
      {
        name: 'Users:Issuer'
        value: 'https://${environmentName}-api.f1mgr.com'
      }
      {
        name: 'Users:Audience'
        value: 'https://${environmentName}-api.f1mgr.com'
      }
      {
        name: 'Users:Secret'
        value: jwtSignatureSecret
      }
      {
        name: 'Teams:AzureStorageAccount'
        value: '@Microsoft.KeyVault(SecretUri=${keyVaultModule.outputs.keyVaultUrl}/secrets/${storageAccountModule.outputs.secretName})'
      }
      {
        name: 'Teams:CacheConnectionString'
        value: '@Microsoft.KeyVault(SecretUri=${keyVaultModule.outputs.keyVaultUrl}/secrets/${redisCacheModule.outputs.secretName})'
      }
      {
        name: 'WEBSITE_RUN_FROM_PACKAGE'
        value: '1'
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

resource certificate 'Microsoft.Web/certificates@2021-01-01' = {
  dependsOn: [
    webAppModule
    keyVaultModule
  ]
  name: 'certificateModule'
  location: resourceGroup().location
  properties: {
    keyVaultId: resourceId('DeployTime', 'Microsoft.Vaults/KeyVault', 'DeployTimeKeyVault')
    keyVaultSecretName: 'f1mgr'
    serverFarmId: appServicePlanModule.outputs.id
  }
}

output webAppName string = webAppName
