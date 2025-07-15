
@description('Name of the resource.')
param name string

@description('Location to deploy the resource. Defaults to the location of the resource group.')
param location string = resourceGroup().location

@description('Tags for the resource.')
param tags object = {}

@description('The resource ID of the virtual network to associate with this resource.')
param vnetId string

@description('The resource ID of the subnet within the virtual network to associate with this resource.')
param subnetId string

@description('The resource ID of the private link service to which the private endpoint will connect.')
param privateLinkServiceId string

@export()
@description('Specifies the type of remote resource to associate with the private endpoint. Supported values are CosmosDb and BlobStorage')
type privateLinkRemoteResourceType = 'CosmosDb' | 'BlobStorage'

@description('Specifies the type of remote resource to associate with the private endpoint. Supported values are CosmosDb and BlobStorage')
param remoteResourceType privateLinkRemoteResourceType = 'CosmosDb'

var privateLinkGroup = remoteResourceType == 'CosmosDb' ? 'Sql' : 'blob'

#disable-next-line no-hardcoded-env-urls
var blobHostName = '.blob.core.windows.net'
var cosmosDBHostName = '.documents.azure.com'

var hostName = remoteResourceType == 'CosmosDb' ? cosmosDBHostName : blobHostName
var privateDnsZoneName = 'privatelink${hostName}'

resource privateEndpoint 'Microsoft.Network/privateEndpoints@2024-07-01' = {
  name: name
  location: location
  tags: tags
  properties: {
    customNetworkInterfaceName: '${name}-nic'
    subnet: {
      id: subnetId
    }
    privateLinkServiceConnections: [
      {
        name: name
        properties: {
          privateLinkServiceId: privateLinkServiceId
          groupIds: [
            privateLinkGroup
          ]
        }
      }
    ]
  }
}

resource privateDnsZone 'Microsoft.Network/privateDnsZones@2024-06-01' = {
  name: 'privatelink${hostName}'
  location: 'global'
  properties: {}
  tags: tags
}

resource privateDnsZoneLink 'Microsoft.Network/privateDnsZones/virtualNetworkLinks@2024-06-01' = {
  parent: privateDnsZone
  name: '${privateDnsZoneName}-link'
  location: 'global'
  properties: {
    registrationEnabled: false
    virtualNetwork: {
      id: vnetId
    }
  }
}

resource privateDnsZoneGroup 'Microsoft.Network/privateEndpoints/privateDnsZoneGroups@2024-05-01' = {
  parent: privateEndpoint
  name: 'default'
  properties: {
    privateDnsZoneConfigs: [
      {
        name: 'privatelink-documents-azure-com'
        properties: {
          privateDnsZoneId: privateDnsZone.id
        }
      }
    ]
  }
}
