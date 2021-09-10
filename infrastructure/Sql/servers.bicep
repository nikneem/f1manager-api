param standardAppName string

@secure()
param sqlServerPassword string = newGuid()

var resourceName = '${standardAppName}-sql'
var adminName = '99424f34-14fa-4172-b535-36f2a6e15488'
var dbName = '${standardAppName}-sqldb'
var connectionString = 'DATA SOURCE=tcp:${resourceName}${environment().suffixes.sqlServerHostname},1433;USER ID=${adminName};PASSWORD=${sqlServerPassword};INITIAL CATALOG=${dbName};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: resourceName
  location: resourceGroup().location
  properties: {
    administratorLogin: adminName
    administratorLoginPassword: sqlServerPassword
  }
}

output sqlServerName string = sqlServer.name
output databaseName string = dbName
output connectionString string = connectionString
output secret object = {
  name: sqlServer.name
  value: connectionString
}

output secretName string = sqlServer.name
