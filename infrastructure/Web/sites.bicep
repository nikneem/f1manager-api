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

var siteConfig = contains(kind, 'linux') ? {
  linuxFxVersion: linuxFxVersion
  alwaysOn: alwaysOn
  ftpsState: ftpState
  http20Enabled: http20Enabled
} : {
  alwaysOn: alwaysOn
  ftpsState: ftpState
  http20Enabled: http20Enabled
}

var linuxFxVersion = 'DOTNET|6.0'

resource webApp 'Microsoft.Web/sites@2020-12-01' = {
  name: resourceName
  location: location
  kind: kind
  properties: {
    serverFarmId: appServicePlanId
    httpsOnly: true
    clientAffinityEnabled: false
    siteConfig: siteConfig
  }
  identity: {
    type: 'SystemAssigned'
  }
}

output servicePrincipal string = webApp.identity.principalId
output webAppName string = webApp.name
