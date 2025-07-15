@description('The name of the Azure Storage Account to create or use.')
param accountName string

@description('Location to deploy the resource. Defaults to the location of the resource group.')
param location string = resourceGroup().location

@description('Tags for the resource.')
param tags object = {}

@export()
type meteringtThroughput = {
  @description('The throughput for the license container')
  licenseContainer: int

  @description('The throughput for the pending license container')
  pendinglicenseContainer: int

  @description('The throughput for the usage container')
  usageContainer: int
}

@description('The throughput for each of the container in metering database.')
param throughput meteringtThroughput

var locations = [
  {
    locationName: location
    failoverPriority: 0
    isZoneRedundant: false
  }
]

resource databaseAccount 'Microsoft.DocumentDB/databaseAccounts@2022-05-15' = {
  name: toLower(accountName)
  kind: 'GlobalDocumentDB'
  location: location
  properties: {
    consistencyPolicy: {
      defaultConsistencyLevel: 'Session'
    }
    locations: locations
    databaseAccountOfferType: 'Standard'
    enableAutomaticFailover: true
    enableMultipleWriteLocations: false
    ipRules: [
      {
        ipAddressOrRange: '161.69.91.46'
      }
      {
        ipAddressOrRange: '161.69.119.32'
      }
      {
        ipAddressOrRange: '208.81.70.37'
      }
      {
        ipAddressOrRange: '185.221.69.37'
      }
      {
        ipAddressOrRange: '4.210.172.107'
      }
      {
        ipAddressOrRange: '13.88.56.148'
      }
      {
        ipAddressOrRange: '13.91.105.215'
      }
      {
        ipAddressOrRange: '40.91.218.243'
      }
    ]
  }
  tags: tags
}

var meteringDatabaseName = 'Metering'
resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2025-04-15' = {
  parent: databaseAccount
  name: meteringDatabaseName
  properties: {
    resource: {
      id: meteringDatabaseName
    }
  }
}

var licenseContainerName = 'License'
resource licenseContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-04-15' = {
  parent: database
  name: licenseContainerName
  properties: {
    resource: {
      id: licenseContainerName
      partitionKey: {
        paths: [
          '/partitionKey'
        ]
        kind: 'Hash'
      }
      indexingPolicy: {
        indexingMode: 'consistent'
        automatic: true
        includedPaths: [
          {
            path: '/*'
          }
        ]
        excludedPaths: [
          {
            path: '/"_etag"/?'
          }
        ]
      }
    }
    options: {
      throughput: throughput.licenseContainer
    }
  }
}

var pendingLicenseContainerName = 'PendingLicense'
resource pendingLicenseContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-04-15' = {
  parent: database
  name: pendingLicenseContainerName
  properties: {
    resource: {
      id: pendingLicenseContainerName
      partitionKey: {
        paths: [
          '/partitionKey'
        ]
        kind: 'Hash'
      }
      indexingPolicy: {
        indexingMode: 'consistent'
        automatic: true
        includedPaths: [
          {
            path: '/*'
          }
        ]
        excludedPaths: [
          {
            path: '/"_etag"/?'
          }
        ]
      }
    }
    options: {
      throughput: throughput.pendinglicenseContainer
    }
  }
}

var usageContainerName = 'Usage'
resource usageContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-04-15' = {
  parent: database
  name: usageContainerName
  properties: {
    resource: {
      id: usageContainerName
      partitionKey: {
        paths: [
          '/partitionKey'
        ]
        kind: 'Hash'
      }
      indexingPolicy: {
        indexingMode: 'consistent'
        automatic: true
        includedPaths: [
          {
            path: '/*'
          }
        ]
        excludedPaths: [
          {
            path: '/"_etag"/?'
          }
        ]
      }
    }
    options: {
      throughput: throughput.usageContainer
    }
  }
}

@description('ID for the deployed Cosmos Db.')
output id string = databaseAccount.id

@description('Name for the deployed Cosmos Db.')
output name string = databaseAccount.name
