@description('Name of the resource.')
param name string
@description('Location to deploy the resource. Defaults to the location of the resource group.')
param location string = resourceGroup().location
@description('Tags for the resource.')
param tags object = {}

@export()
@description('SKU information for Storage Account.')
type storageAccountSkuInfo = {
  @description('Name of the SKU.')
  name:
    | 'Premium_LRS'
    | 'Premium_ZRS'
    | 'Standard_GRS'
    | 'Standard_GZRS'
    | 'Standard_LRS'
    | 'Standard_RAGRS'
    | 'Standard_RAGZRS'
    | 'Standard_ZRS'
}

@description('Storage Account SKU. Defaults to Standard_LRS.')
param sku storageAccountSkuInfo = {
  name: 'Standard_LRS'
}

resource storage 'Microsoft.Storage/storageAccounts@2024-01-01' = {
  name: name
  location: location
  sku: sku
  kind: 'StorageV2'
  tags: tags
  properties: {
    allowBlobPublicAccess: false
    networkAcls: {
      ipRules: [
        {
          value: '84.246.168.49'
          action: 'Allow'
        }
        {
          value: '161.69.71.42'
          action: 'Allow'
        }
        {
          value: '208.81.67.37'
          action: 'Allow'
        }
        {
          value: '185.221.69.37'
          action: 'Allow'
        }
        {
          value: '208.65.145.37'
          action: 'Allow'
        }
        {
          value: '185.221.71.37'
          action: 'Allow'
        }
        {
          value: '208.81.70.37'
          action: 'Allow'
        }
        {
          value: '84.246.168.11'
          action: 'Allow'
        }
      ]
      defaultAction: 'Deny'
    }
  }
}

@description('ID for the deployed Storage Account.')
output id string = storage.id

@description('Name for the deployed Storage Account.')
output name string = storage.name

@secure()
@description('The primary connection string for the Azure Storage Account Blob service.')
output blobPrimaryConnection string = 'DefaultEndpointsProtocol=https;AccountName=${storage.name};EndpointSuffix=${environment().suffixes.storage};AccountKey=${storage.listKeys().keys[0].value}'
