targetScope = 'subscription'

param systemName string = 'F1Man-Api'
@allowed([
  'Dev'
  'Test'
  'Acc'
  'Prod'
])
param environmentName string = 'Dev'
param azureRegion object = {
  location: 'westeurope'
  abbreviation: 'weu'
}
param developerObjectIds array = [
  'ce00c98d-c389-47b0-890e-7f156f136ebd'
]
@secure()
param jwtSignatureSecret string = newGuid()

param basicAppSettings array = [
  {
    name: 'WEBSITE_RUN_FROM_PACKAGE'
    value: '1'
  }
]

var tables = [
  'Users'
  'Logins'
  'RefreshTokens'
  'Components'
]

var resourceGroupName = '${systemName}-${environmentName}-${azureRegion.abbreviation}'
var standardAppName = toLower('${systemName}-${environmentName}-${azureRegion.abbreviation}')

resource targetResourceGroup 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  location: deployment().location
  name: resourceGroupName
}

resource eventGrid 'Microsoft.EventGrid/domains@2021-06-01-preview' existing = {
  name: '${systemName}-${environmentName}-${azureRegion}-eg'
  scope: resourceGroup('F1Manager-${environmentName}-Integration')
}

resource deployTimeKeyVault 'Microsoft.KeyVault/vaults@2021-04-01-preview' existing = {
  name: 'f1man-deploytime-kv'
  scope: resourceGroup('F1Manager-DeployTime')
}

module redisCacheModule 'Cache/redis.bicep' = {
  name: 'redisCacheModule'
  scope: targetResourceGroup
  params: {
    standardAppName: standardAppName
  }
}

module storageAccountModule 'Storage/storageAccounts.bicep' = {
  name: 'storageAccountModule'
  scope: targetResourceGroup
  params: {
    standardAppName: standardAppName
  }
}

module storageAccountTables 'Storage/tableServices/tables.bicep' = {
  dependsOn: [
    storageAccountModule
  ]
  scope: targetResourceGroup
  name: 'storageAccountTables'
  params: {
    storageAccountName: storageAccountModule.outputs.storageAccountName
    tableNames: tables
  }
}

module applicationInsightsModule 'Insights/components.bicep' = {
  name: 'applicationInsightsDeploy'
  scope: targetResourceGroup
  params: {
    standardAppName: standardAppName
  }
}

module keyVaultModule 'KeyVault/vaults.bicep' = {
  name: 'keyVaultDeploy'
  scope: targetResourceGroup
  params: {
    standardAppName: standardAppName
  }
}

module sqlServerModule 'Sql/servers.bicep' = {
  name: 'sqlServerModule'
  scope: targetResourceGroup
  params: {
    standardAppName: standardAppName
    sqlServerPassword: deployTimeKeyVault.getSecret('SqlServerPassword')
  }
}

module sqlServerDatabaseModule 'Sql/servers/database.bicep' = {
  dependsOn: [
    sqlServerModule
  ]
  name: 'sqlServerDatabaseModule'
  scope: targetResourceGroup
  params: {
    databaseName: sqlServerModule.outputs.databaseName
    sqlServerName: sqlServerModule.outputs.sqlServerName
  }
}

module keyVaultSecretsModule 'KeyVault/vaults/secrets.bicep' = {
  dependsOn: [
    keyVaultModule
    storageAccountModule
    sqlServerModule
    redisCacheModule
  ]
  name: 'keyVaultSecrets'
  scope: targetResourceGroup
  params: {
    keyVault: keyVaultModule.outputs.keyVaultName
    secrets: union(storageAccountModule.outputs.secret, sqlServerModule.outputs.secret, redisCacheModule.outputs.secret)
  }
}

module appServicePlanModule 'Web/serverFarms.bicep' = {
  name: 'appServicePlanDeploy'
  scope: targetResourceGroup
  params: {
    standardAppName: standardAppName
  }
}

module webAppModule 'Web/sites.bicep' = {
  dependsOn: [
    appServicePlanModule
  ]
  name: 'webAppModule'
  scope: targetResourceGroup
  params: {
    standardAppName: standardAppName
    appServicePlanId: appServicePlanModule.outputs.id
  }
}

module keyVaultAccessPolicyModule 'KeyVault/vaults/accessPolicies.bicep' = {
  name: 'keyVaultAccessPolicyDeploy'
  dependsOn: [
    keyVaultModule
    webAppModule
  ]
  scope: targetResourceGroup
  params: {
    keyVaultName: keyVaultModule.outputs.keyVaultName
    principalId: webAppModule.outputs.servicePrincipal
  }
}

@batchSize(1)
module developerAccessPolicies 'KeyVault/vaults/accessPolicies.bicep' = [for developer in developerObjectIds: {
  name: 'developer${developer}'
  dependsOn: [
    keyVaultModule
    keyVaultAccessPolicyModule
  ]
  scope: targetResourceGroup
  params: {
    keyVaultName: keyVaultModule.outputs.keyVaultName
    principalId: developer
  }
}]

module websiteConfiguration 'Web/sites/config.bicep' = {
  name: 'websiteConfiguration'
  dependsOn: [
    keyVaultModule
    keyVaultAccessPolicyModule
    keyVaultSecretsModule
  ]
  scope: targetResourceGroup
  params: {
    webAppName: webAppModule.outputs.webAppName
    appSettings: union(basicAppSettings, applicationInsightsModule.outputs.appConfiguration, keyVaultSecretsModule.outputs.keyVaultReferences)
  }
}
