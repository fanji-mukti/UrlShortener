@description('Name of the resource.')
param name string
@description('Location to deploy the resource. Defaults to the location of the resource group.')
param location string = resourceGroup().location
@description('Tags for the resource.')
param tags object = {}

@description('ID for the App Service Plan associated with the Function App.')
param appServicePlanId string
@description('Public network access for the Function App. Defaults to Enabled.')
param publicNetworkAccess string = 'Enabled'
@description('Always On setting for the Function App. Defaults to false.')
param alwaysOn bool = false
@description('The resource ID of the subnet within the virtual network to be used for regional VNet integration with the Function App.')
param virtualNetworkSubnetId string

@secure()
@description('The connection string for the Azure Storage Account to be used by this resource.')
param storageAccountConnection string

resource functionApp 'Microsoft.Web/sites@2024-11-01' = {
  name: name
  location: location
  tags: tags
  kind: 'functionapp'
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    serverFarmId: appServicePlanId
    siteConfig: {
      alwaysOn: alwaysOn
    }
    httpsOnly: true
    publicNetworkAccess: publicNetworkAccess
    virtualNetworkSubnetId: virtualNetworkSubnetId
  }
}

resource config 'Microsoft.Web/sites/config@2024-11-01' = {
  parent: functionApp
  name: 'appsettings'
  properties: {
    AzureWebJobsStorage: storageAccountConnection
    FUNCTIONS_EXTENSION_VERSION: '~4'
    FUNCTIONS_WORKER_RUNTIME: 'dotnet-isolated'
    DOTNET_VERSION: '8.0'
  }
}


@description('ID for the deployed Function App resource.')
output id string = functionApp.id
@description('Name for the deployed Function App resource.')
output name string = functionApp.name
@description('Host for the deployed Function App resource.')
output host string = functionApp.properties.defaultHostName
@description('The System Assigned Managed Identity of the Function App.')
output principalId string = functionApp.identity.principalId
