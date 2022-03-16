param standardAppName string

@allowed([
  'Premium_LRS'
  'Premium_ZRS'
  'Standard_GRS'
  'Standard_GZRS'
  'Standard_LRS'
  'Standard_RAGRS'
  'Standard_RAGZRS'
  'Standard_ZRS'
])
param skuName string = 'Standard_LRS'

param location string = resourceGroup().location

param tableNames array = []
param containerNames array = []

var resourceName = toLower(replace(standardAppName, '-', ''))

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: resourceName
  kind: 'StorageV2'
  location: location
  sku: {
    name: skuName
  }
}

module tables 'tableServices/tables.bicep' = {
  name: 'storageTables'
  params: {
    storageAccountName: storageAccount.name
    tableNames: tableNames
  }
}

module containers 'blobServices/containers.bicep' = {
  name: 'storageContainers'
  params: {
    storageAccountName: storageAccount.name
    containerNames: containerNames
  }
}

output connectionString string = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
output secret object = {
  name: 'AzureStorageAccount'
  value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
}

output secretName string = 'AzureStorageAccount'
output storageAccountName string = storageAccount.name
