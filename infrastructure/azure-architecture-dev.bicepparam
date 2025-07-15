using 'azure-architecture.bicep'

param environmentName = 'dev'

param location = 'uksouth'

param tagsValue = {
    CostString: '2000.GB.371.403039'
    Product: 'EDQ SaaS Platform'
    Environment: environmentName
    Component: 'SaaS Metering'
    AppID: '2082'
    Category: 'EdqPlatform'
    Program: 'Platform Services'
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


param meteringAppServicePlanSku = {
  name: 'S1'
}

param meteringAppServicePlanKind = 'app'
