param keyVault string
param secret object

resource secretsLoop 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault}/${secret.name}'
  properties: {
    value: secret.value
  }
}

output keyVaultReference array = [
  {
    name: secret.name
    value: '@Microsoft.KeyVault(SecretUri=https://${keyVault}${environment().suffixes.keyvaultDns}/secrets/${secret.name})'
  }
]
