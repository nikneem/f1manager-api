param storageAccountName string
param blobServiceName string = 'default'
param containerNames array

resource storageContainers 'Microsoft.Storage/storageAccounts/blobServices/containers@2021-08-01' = [for containerName in containerNames: {
  name: '${storageAccountName}/${blobServiceName}/${containerName.name}'
  properties: {
    publicAccess: containerName.publicAccess
  }
}]
