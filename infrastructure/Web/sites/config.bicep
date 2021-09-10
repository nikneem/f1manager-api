param webAppName string
param appSettings array

resource config 'Microsoft.Web/sites/config@2020-12-01' = {
  name: '${webAppName}/web'
  location: resourceGroup().location
  properties: {
    alwaysOn: true
    ftpState: 'Disabled'
    appSettings: [for setting in appSettings: {
      name: setting.name
      value: setting.value
    }]
  }
}
