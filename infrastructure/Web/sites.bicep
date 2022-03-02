param standardAppName string
param appServicePlanId string
param location string = resourceGroup().location

@allowed([
  'app'
  'app,linux'
  'functionapp'
  'functionapp,linux'
])
param kind string = 'app'

param http20Enabled bool = true
param alwaysOn bool = true
@allowed([
  'Disabled'
  'AllAllowed'
  'FtpsOnly'
])
param ftpState string = 'Disabled'

var resourceName = '${standardAppName}-app'

resource webApp 'Microsoft.Web/sites@2020-12-01' = {
  name: resourceName
  location: location
  kind: kind
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    clientAffinityEnabled: false
    siteConfig: {
      alwaysOn: alwaysOn
      ftpsState: ftpState
      http20Enabled: http20Enabled
    }
  }
  identity: {
    type: 'SystemAssigned'
  }
}

output servicePrincipal string = webApp.identity.principalId
output webAppName string = webApp.name
