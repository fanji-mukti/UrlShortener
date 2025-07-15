@description('The name of the Azure Storage Account to create or use.')
param accountName string

@description('Location to deploy the resource. Defaults to the location of the resource group.')
param location string = resourceGroup().location

@description('Tags for the resource.')
param tags object = {}

@export()
type UrlShortenerStoreThrougput = {
  @description('The throughput for the shortened url container')
  shortenedUrlContainer: int

  @description('The throughput for the original url container')
  originalUrlContainer: int
}

@description('The throughput for each of the container in metering database.')
param throughput UrlShortenerStoreThrougput

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
    publicNetworkAccess: 'Disabled'
  }
  tags: tags
}

var databaseName = 'UrlShortenerStore'
resource database 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases@2025-04-15' = {
  parent: databaseAccount
  name: databaseName
  properties: {
    resource: {
      id: databaseName
    }
  }
}

var shortenedUrlContainerName = 'ShortenedUrl'
resource shortenedUrlContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-04-15' = {
  parent: database
  name: shortenedUrlContainerName
  properties: {
    resource: {
      id: shortenedUrlContainerName
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
      throughput: throughput.shortenedUrlContainer
    }
  }
}

var originalUrlContainerName = 'OriginalUrl'
resource originalUrlContainer 'Microsoft.DocumentDB/databaseAccounts/sqlDatabases/containers@2025-04-15' = {
  parent: database
  name: originalUrlContainerName
  properties: {
    resource: {
      id: originalUrlContainerName
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
      throughput: throughput.originalUrlContainer
    }
  }
}

@description('ID for the deployed Cosmos Db.')
output id string = databaseAccount.id

@description('Name for the deployed Cosmos Db.')
output name string = databaseAccount.name
