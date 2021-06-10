param storageAccountName string
param tableServicesName string = 'default'
param tableNames array

resource tableResource 'Microsoft.Storage/storageAccounts/tableServices/tables@2021-02-01' = [for tableName in tableNames: {
  name: '${storageAccountName}/${tableServicesName}/${tableName}'
}]
