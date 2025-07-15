# UrlShortener

UrlShortener is a cloud-native URL shortening service built with Azure Functions, .NET 8, and Azure Cosmos DB. It provides REST APIs to shorten URLs and redirect users to the original URLs.

## Features

- Shorten long URLs to compact, shareable links
- Redirect from short URLs to original URLs
- Optional expiration for shortened URLs
- Built-in integration with Azure Cosmos DB for scalable storage
- Infrastructure-as-Code using Bicep for Azure deployment

## Solution Structure

- `src/Core/` - Core logic, models, and repository abstractions
- `src/Function/` - Azure Functions HTTP and Cosmos DB triggers
- `src/Core.UnitTests/` - Unit tests for core logic
- `src/Function.Tests/` - Integration tests for Azure Functions
- `infrastructure/` - Bicep modules for Azure infrastructure

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [Azure Functions Core Tools](https://docs.microsoft.com/azure/azure-functions/functions-run-local)
- [Azure CLI](https://docs.microsoft.com/cli/azure/install-azure-cli)
- [Docker](https://www.docker.com/) (for local Cosmos DB emulator)

### Local Development

1. **Start Cosmos DB Emulator**  
   You can use the Azure Cosmos DB Emulator or a local Cosmos DB instance.

2. **Configure Local Settings**  
   Update `src/Function/local.settings.json` with your Cosmos DB connection string and other settings.

3. **Run the Function App Locally**
   ```sh
   cd src/Function
   func start
   ```

4. **Run Tests**
   ```sh
   dotnet test ../Core.UnitTests
   dotnet test ../Function.Tests
   ```

### API Endpoints

- **Shorten URL**  
  `POST /api/v1/data/shorten`  
  Request body:
  ```json
  {
    "originalUrl": "https://www.example.com/",
    "expiresAt": "2025-01-01T00:00:00Z" // optional
  }
  ```

- **Redirect**  
  `GET /api/{shortUrl}`

## Deployment

Infrastructure is defined in [infrastructure/](infrastructure/) using Bicep.  
To deploy:

```sh
az deployment sub create \
  --location <location> \
  --template-file infrastructure/azure-architecture.bicep \
  --parameters environmentName=dev location=<location>
```