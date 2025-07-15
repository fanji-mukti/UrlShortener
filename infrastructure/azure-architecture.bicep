import { skuInfo } from 'modules/app-service-plan.bicep'
import { serviceEndpoint } from 'modules/virtual-network-subnet.bicep'

targetScope = 'subscription'

@description('Specifies the deployment environment (e.g., dev, stg, prd) for resource provisioning.')
@allowed([ 'dev', 'devshared', 'stg', 'prd' ])
param environmentName string

@description('Location to deploy the resource group.')
param location string

@description('A set of key-value pairs to assign as tags to all deployed resources.')
param tagsValue object

@description('App Service Plan SKU.')
param appServicePlanSku skuInfo

@description('App Service Plan Kind.')
param appServicePlanKind string

@description('Address blocks reserved for this virtual network in CIDR notation.')
param virtualNetworkAddressPrefixes string[]

@description('Address blocks reserved for this app service subnet.')
param appSubNetaddressPrefix string

@description('List of address blocks reserved for this database subnet.')
param databaseSubNetaddressPrefix string

@description('List of service endpoint in database subnet.')
param databaseSubnetServiceEndpoints serviceEndpoint[]

resource resourceGroup 'Microsoft.Resources/resourceGroups@2025-04-01' = {
  name: 'url-shortener-${environmentName}-rg'
  location: location
  tags: tagsValue
}

module virtualNetwork 'modules/virtual-network.bicep' = {
  scope: resourceGroup
  params: {
    name: 'url-shortener-${location}-${environmentName}-vnet'
    location: location
    addressPrefixes: virtualNetworkAddressPrefixes
    tags: tagsValue
  }
}

module appSubnet 'modules/virtual-network-subnet.bicep' = {
  scope: resourceGroup
  params: {
    name: 'url-shortener-${location}-${environmentName}-snet-app'
    addressPrefix: appSubNetaddressPrefix
    serviceEndpoints: []
    virtualNetworkName: virtualNetwork.outputs.name
    delegations: [
      {
        name: 'Microsoft.Web/serverFarms'
        properties: {
          serviceName: 'Microsoft.Web/serverFarms'
        }
      }
    ]
  }
}

module databaseSubnet 'modules/virtual-network-subnet.bicep' = {
  scope: resourceGroup
  params: {
    name: 'url-shortener-${location}-${environmentName}-snet-database'
    addressPrefix: databaseSubNetaddressPrefix
    serviceEndpoints: databaseSubnetServiceEndpoints
    virtualNetworkName: virtualNetwork.outputs.name
  }
}

module cosmosDb 'modules/cosmos-db.bicep' = {
  scope: resourceGroup
  params: {
    accountName: 'url-shortener-${environmentName}-cosmos'
    throughput: {
      licenseContainer: 400
      pendinglicenseContainer: 400
      usageContainer: 800
    }
    tags: tagsValue
  }
}

module cosmosDbPrivateEndpoint 'modules/private-endpoint.bicep' = {
  scope: resourceGroup
  params: {
    name: 'url-shortener-${location}-${environmentName}-pep-cosmosdb'
    location: location
    privateLinkServiceId: cosmosDb.outputs.id
    vnetId: virtualNetwork.outputs.id
    subnetId: databaseSubnet.outputs.id
    remoteResourceType: 'CosmosDb'
    tags: tagsValue
  }
}

module appServicePlan 'modules/app-service-plan.bicep' = {
  scope: resourceGroup
  params: {
    name: 'url-shortener-${location}-${environmentName}-asp'
    location: location
    sku: appServicePlanSku
    kind: appServicePlanKind
    tags: tagsValue
  }
}

module storageAccount 'modules/storage-account.bicep' = {
  scope: resourceGroup
  params: {
    name: 'urlshortener${environmentName}sa'
    location: location
    tags: tagsValue
    sku: {
      name: 'Standard_LRS'
    }
  }
}

module blobPrivateEndpoint 'modules/private-endpoint.bicep' = {
  scope: resourceGroup
  params: {
    name: 'url-shortener-${location}-${environmentName}-pep-blobstorage'
    location: location
    privateLinkServiceId: storageAccount.outputs.id
    vnetId: virtualNetwork.outputs.id
    subnetId: databaseSubnet.outputs.id
    remoteResourceType: 'BlobStorage'
    tags: tagsValue
  }
}

module functionApp 'modules/function-app.bicep' = {
  scope: resourceGroup
  params: {
    name: 'url-shortener-${location}-${environmentName}-funcapp'
    appServicePlanId: appServicePlan.outputs.id
    alwaysOn: true
    storageAccountConnection: storageAccount.outputs.blobPrimaryConnection
    virtualNetworkSubnetId: appSubnet.outputs.id
    tags: tagsValue
  }
}
