param standardAppName string
param logAnalyticsResourceId string
param targetLocation string = resourceGroup().location

var resourceName = '${standardAppName}-ai'

resource applicationInsights 'Microsoft.Insights/components@2020-02-02-preview' = {
  kind: 'web'
  location: targetLocation
  name: resourceName
  properties: {
    Application_Type: 'web'
    IngestionMode: 'LogAnalytics'
    WorkspaceResourceId: logAnalyticsResourceId
  }
}

output applicationInsightsName string = applicationInsights.name
output instrumentationKey string = applicationInsights.properties.InstrumentationKey
output appConfiguration array = [
  {
    name: 'APPINSIGHTS_INSTRUMENTATIONKEY'
    value: applicationInsights.properties.InstrumentationKey
  }
  {
    name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
    value: applicationInsights.properties.ConnectionString
  }
  {
    name: 'APPINSIGHTS_PROFILERFEATURE_VERSION'
    value: '1.0.0'
  }
  {
    name: 'APPINSIGHTS_SNAPSHOTFEATURE_VERSION'
    value: '1.0.0'
  }
]
