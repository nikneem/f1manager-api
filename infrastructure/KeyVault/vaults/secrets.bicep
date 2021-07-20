param keyVault string
param secrets array

@batchSize(1)
resource secretsLoop 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = [for secret in secrets: {
  name: '${keyVault}/${secret.name}'
  properties: {
    value: secret.value
  }
}]

output keyVaultReferences array = [for secret in secrets: {
  name: secret
  value: '@Microsoft.KeyVault(SecretUri=https://${keyVault}${environment().suffixes.keyvaultDns}/secrets/${secret})'
}]
