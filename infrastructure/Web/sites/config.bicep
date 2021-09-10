param webAppName string
param appSettings array

resource config 'Microsoft.Web/sites/config@2020-12-01' = {
  name: '${webAppName}/web'
  properties: {
    alwaysOn: true
    ftpState: 'Disabled'
    appSettings: appSettings
  }
}
