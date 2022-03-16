param keyVault string
param name string
@secure()
param value string
param configurationName string

resource secretsLoop 'Microsoft.KeyVault/vaults/secrets@2021-04-01-preview' = {
  name: '${keyVault}/${name}'
  properties: {
    value: value
  }
}

output keyVaultReference array = [
  {
    name: configurationName
    value: '@Microsoft.KeyVault(SecretUri=https://${keyVault}${environment().suffixes.keyvaultDns}/secrets/${name})'
  }
]
