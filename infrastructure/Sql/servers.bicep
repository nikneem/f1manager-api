param standardAppName string

@secure()
param sqlServerPassword string = newGuid()

param location string = resourceGroup().location

var resourceName = '${standardAppName}-sql'
var adminName = 'phfqommi7mbwg'
var dbName = '${standardAppName}-sqldb'
var connectionString = 'DATA SOURCE=tcp:${resourceName}${environment().suffixes.sqlServerHostname},1433;USER ID=${adminName};PASSWORD=${sqlServerPassword};INITIAL CATALOG=${dbName};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;'

resource sqlServer 'Microsoft.Sql/servers@2021-02-01-preview' = {
  name: resourceName
  location: location
  properties: {
    administratorLogin: adminName
    administratorLoginPassword: sqlServerPassword
  }
}

resource internalAzureTraffic 'Microsoft.Sql/servers/firewallRules@2021-08-01-preview' = {
  name: 'AllowAllWindowsAzureIps'
  parent: sqlServer
  properties: {
    startIpAddress: '0.0.0.0'
    endIpAddress: '0.0.0.0'
  }
}

output sqlServerName string = sqlServer.name
output databaseName string = dbName
output connectionString string = connectionString
output secret object = {
  name: 'SqlConnectionString'
  value: connectionString
}

output secretName string = sqlServer.name
