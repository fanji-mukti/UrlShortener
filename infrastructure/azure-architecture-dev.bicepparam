using 'azure-architecture.bicep'

param environmentName = 'dev'

param location = 'uksouth'

param tagsValue = {
    Environment: environmentName
}

param virtualNetworkAddressPrefixes = ['10.0.0.0/24']

param appSubNetaddressPrefix = '10.0.0.0/28'

param databaseSubNetaddressPrefix = '10.0.0.16/28'

param databaseSubnetServiceEndpoints = [
    {
        locations: [ '*' ]
        service: 'Microsoft.AzureCosmosDB'
    }
]


param appServicePlanSku = {
  name: 'S1'
}

param appServicePlanKind = 'app'
