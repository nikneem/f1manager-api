param webAppName string
param appServicePlanId string

resource webApp 'Microsoft.Web/sites@2020-12-01' = {
  name: webAppName
  location: resourceGroup().location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    clientAffinityEnabled: false
    siteConfig: {
      alwaysOn: true
      ftpsState: 'Disabled'
      netFrameworkVersion: 'v5.0'
      http20Enabled: true
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

output servicePrincipal string = webApp.identity.principalId
output webAppName string = webAppName
