param webAppName string
param appSettings array
param alwaysOn bool = true

resource config 'Microsoft.Web/sites/config@2020-12-01' = {
  name: '${webAppName}/web'
  properties: {
    alwaysOn: alwaysOn
    ftpState: 'Disabled'
    appSettings: appSettings
    netFrameworkVersion: 'v6.0'
  }
}
