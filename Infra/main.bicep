targetScope='subscription'

resource rubbedrg 'Microsoft.Resources/resourceGroups@2022-09-01' = {
  name: 'rubbed-rg'
  location: 'WestEurope'
}

module appModule 'app.bicep' = {
  name: 'appModule'
  scope: rubbedrg
}
