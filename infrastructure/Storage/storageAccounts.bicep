param standardAppName string

@allowed([
  'Standard'
  'Premium'
])
param skuTier string = 'Standard'

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

var resourceName = toLower(replace(standardAppName, '-', ''))

resource storageAccount 'Microsoft.Storage/storageAccounts@2021-02-01' = {
  name: resourceName
  kind: 'StorageV2'
  location: resourceGroup().location
  sku: {
    name: skuName
    tier: skuTier
  }
}

output connectionString string = 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
output secret object = {
  name: 'AzureStorageAccount'
  value: 'DefaultEndpointsProtocol=https;AccountName=${storageAccount.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${listKeys(storageAccount.id, storageAccount.apiVersion).keys[0].value}'
}

output secretName string = 'AzureStorageAccount'
output storageAccountName string = storageAccount.name
