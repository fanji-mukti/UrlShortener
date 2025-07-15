@description('Name of the resource.')
param name string
@description('Location to deploy the resource. Defaults to the location of the resource group.')
param location string = resourceGroup().location
@description('Tags for the resource.')
param tags object = {}

@description('ID for the App Service Plan associated with the Web App.')
param appServicePlanId string
@description('Web App Kind. Defaults to app,linux.')
@allowed([ 'app', 'app,linux' ])
param kind string = 'app,linux'
@description('Public network access for the Web App. Defaults to Enabled.')
param publicNetworkAccess string = 'Enabled'
@description('Specifies whether the App Service should always be running, even when there are no requests. Enabling Always On prevents the app from being unloaded due to inactivity. default to true.')
param alwaysOn bool = true
@description('The resource ID of the subnet within the virtual network to be used for regional VNet integration with the App Service.')
param virtualNetworkSubnetId string

resource webApp 'Microsoft.Web/sites@2024-11-01' = {
  name: name
  location: location
  tags: tags
  kind: kind
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

@description('ID for the deployed Web App resource.')
output id string = webApp.id
@description('Name for the deployed Web App resource.')
output name string = webApp.name
@description('Host for the deployed Web App resource.')
output host string = webApp.properties.defaultHostName

