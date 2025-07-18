@description('Name of the resource.')
param name string
@description('Location to deploy the resource. Defaults to the location of the resource group.')
param location string = resourceGroup().location
@description('Tags for the resource.')
param tags object = {}

@export()
@description('SKU information for App Service Plan.')
type skuInfo = {
  @description('Name of the SKU.')
  name: 'F1' | 'D1' | 'B1' | 'B2' | 'B3' | 'S1' | 'S2' | 'S3' | 'P1' | 'P2' | 'P3' | 'I1' | 'I2' | 'I3' | 'Y1'
}

@description('App Service Plan SKU. Defaults to Y1.')
param sku skuInfo = {
  name: 'Y1'
}

@description('App Service Plan Kind. Defaults to linux.')
@allowed([
  'app'
  'linux'
])
param kind string = 'linux'

resource appServicePlan 'Microsoft.Web/serverfarms@2024-11-01' = {
  name: name
  location: location
  tags: tags
  sku: sku
  kind: kind
}

@description('ID for the deployed App Service Plan resource.')
output id string = appServicePlan.id
@description('Name for the deployed App Service Plan resource.')
output name string = appServicePlan.name
