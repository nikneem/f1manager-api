param standardAppName string
param targetLocation string = resourceGroup().location

var resourceName = '${standardAppName}-la'

resource logAnalyticsWorkSpaces 'Microsoft.OperationalInsights/workspaces@2021-12-01-preview' = {
  location: targetLocation
  name: resourceName
  properties: {
    retentionInDays: 15
  }
}

output resourceId string = logAnalyticsWorkSpaces.id
