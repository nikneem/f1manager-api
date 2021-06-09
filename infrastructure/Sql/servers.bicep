param systemName string = 'f1man'
@allowed([
  'dev'
  'test'
  'acc'
  'prod'
])
param environmentName string = 'prod'
param azureRegion string = 'weu'

@secure()
param sqlServerPassword string = newGuid()

var sqlServerName = '${systemName}-${environmentName}-${azureRegion}-sqlserver'
var databaseName = '${systemName}-${environmentName}-${azureRegion}-database'
var adminName = '${systemName}${environmentName}${azureRegion}-admin'
var connectionString = 'DATA SOURCE=tcp:${sqlServerName}${environment().suffixes.sqlServerHostname},1433;USER ID=${adminName};PASSWORD=${sqlServerPassword};INITIAL CATALOG=${databaseName};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: sqlServerName
  location: resourceGroup().location
  properties: {
    administratorLogin: adminName
    administratorLoginPassword: sqlServerPassword
  }
}

output sqlServerName string = sqlServerName
output databaseName string = databaseName
output connectionString string = connectionString
output secret array = [
  {
    name: sqlServerName
    value: connectionString
  }
]
output secretName string = sqlServerName
