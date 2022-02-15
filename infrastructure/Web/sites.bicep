param standardAppName string
param appServicePlanId string

var resourceName = '${standardAppName}-app'

resource webApp 'Microsoft.Web/sites@2020-12-01' = {
  name: resourceName
  location: resourceGroup().location
  kind: 'app'
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    clientAffinityEnabled: false
    siteConfig: {
      alwaysOn: true
      ftpsState: 'Disabled'
      http20Enabled: true
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

output servicePrincipal string = webApp.identity.principalId
output webAppName string = webApp.name
