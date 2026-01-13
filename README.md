# Panda.NuGet.AsanaClient

A modern, strongly-typed .NET client library for the [Asana API](https://developers.asana.com/docs), generated using Microsoft Kiota. This library provides full async/await support, dependency injection integration, and leverages HttpClientFactory for optimal performance.

## Features

- ✅ **Strongly-typed API client** - Full IntelliSense support with generated models
- ✅ **Modern async/await** - Built for .NET 10.0 with modern C# patterns
- ✅ **Dependency Injection** - First-class support for ASP.NET Core DI
- ✅ **HttpClientFactory** - Proper HttpClient lifecycle management
- ✅ **Kiota-generated** - Auto-generated from official Asana OpenAPI specification
- ✅ **Easy authentication** - Simple Bearer token authentication
- ✅ **Extensible** - Support for custom authentication providers

## Important Notice

> **Unofficial Library:** This is an unofficial community-maintained library and is not affiliated with or endorsed by Asana. There is no guarantee for future maintenance or updates. Use at your own discretion.

## Installation

```bash
dotnet add package Panda.NuGet.AsanaClient
```

## Quick Start

### Basic Usage (without DI)

```csharp
using Panda.NuGet.AsanaClient;

// Create HttpClient (ideally from IHttpClientFactory)
using var httpClient = new HttpClient();

// Create the Asana client with your Personal Access Token
var asanaClient = new AsanaApiClient("your-asana-personal-access-token", httpClient);

// Use the client to interact with Asana API
var workspaces = await asanaClient.Client.Workspaces.GetAsync();

foreach (var workspace in workspaces.Data)
{
    Console.WriteLine($"Workspace: {workspace.Name}");
}
```

### ASP.NET Core / Dependency Injection

**1. Register the client in `Program.cs`:**

```csharp
using Panda.NuGet.AsanaClient;

var builder = WebApplication.CreateBuilder(args);

// Option 1: Register with access token from configuration
builder.Services.AddAsanaClient(
    builder.Configuration["Asana:AccessToken"]!
);

// Option 2: Register with token factory (for dynamic token retrieval)
builder.Services.AddAsanaClient(sp =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    return config["Asana:AccessToken"]!;
});

// Option 3: Register both high-level and low-level clients
builder.Services.AddAsanaClientWithApiClient(
    builder.Configuration["Asana:AccessToken"]!
);

var app = builder.Build();
```

**2. Inject and use in your services:**

```csharp
public class AsanaService
{
    private readonly AsanaApiClient _asanaClient;

    public AsanaService(AsanaApiClient asanaClient)
    {
        _asanaClient = asanaClient;
    }

    public async Task<List<WorkspaceInfo>> GetWorkspacesAsync()
    {
        var response = await _asanaClient.Client.Workspaces.GetAsync();

        return response.Data
            .Select(w => new WorkspaceInfo
            {
                Id = w.Gid,
                Name = w.Name
            })
            .ToList();
    }

    public async Task CreateTaskAsync(string projectId, string taskName)
    {
        await _asanaClient.Client.Tasks.PostAsync(new TasksPostRequestBody
        {
            Data = new TasksPostRequestBody_data
            {
                Name = taskName,
                Projects = new List<string> { projectId }
            }
        });
    }
}
```

### Configuration in appsettings.json

```json
{
  "Asana": {
    "AccessToken": "your-personal-access-token-here"
  }
}
```

**⚠️ Security Note:** Never commit access tokens to source control. Use User Secrets for development and secure key vaults for production.

```bash
# Set up user secrets for development
dotnet user-secrets init
dotnet user-secrets set "Asana:AccessToken" "your-token-here"
```

## Advanced Usage

### Custom Authentication Provider

```csharp
using Microsoft.Kiota.Abstractions.Authentication;

public class CustomAuthProvider : IAuthenticationProvider
{
    public Task AuthenticateRequestAsync(
        RequestInformation request,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        // Your custom authentication logic
        request.Headers.Add("Authorization", $"Bearer {GetToken()}");
        return Task.CompletedTask;
    }
}

// Register with DI
builder.Services.AddAsanaClient(sp => new CustomAuthProvider());
```

### Accessing the Low-Level Kiota Client

The `AsanaApiClient` exposes the underlying Kiota-generated `ApiClient` through the `Client` property for full control:

```csharp
// High-level wrapper
var asanaClient = serviceProvider.GetRequiredService<AsanaApiClient>();

// Low-level Kiota client
var apiClient = asanaClient.Client;

// Full access to all endpoints
var tasks = await apiClient.Tasks.GetAsync(requestConfiguration =>
{
    requestConfiguration.QueryParameters.Project = projectId;
    requestConfiguration.QueryParameters.Limit = 50;
});
```

### Extended Properties for Task Creation

The auto-generated Asana OpenAPI specification is incomplete and missing common properties like `Name` and `Notes` for task creation. This library includes **partial class extensions** that add these missing properties for a better developer experience:

```csharp
var taskRequest = new TasksPostRequestBody
{
    Data = new TasksPostRequestBody_data
    {
        // ✅ Extended properties (not in OpenAPI spec, but fully supported)
        Name = "My Task",              // Task name
        Notes = "Task description",    // Plain text notes
        HtmlNotes = "<p>Rich text</p>", // HTML formatted notes
        DueOn = "2026-01-20",          // Due date (YYYY-MM-DD)
        DueAt = "2026-01-20T15:00:00Z", // Due date with time (ISO 8601)
        StartOn = "2026-01-13",        // Start date
        Completed = false,             // Completion status

        // Standard properties from OpenAPI spec
        Assignee = "user-gid",
        Projects = new List<string> { "project-gid" },
        Workspace = "workspace-gid"
    }
};
```

The `Extensions/` directory contains partial class extensions that add these commonly-used properties which are missing from the auto-generated OpenAPI specification.


This library supports the complete Asana API including:

- **Workspaces** - List and manage workspaces
- **Projects** - Create, update, and query projects
- **Tasks** - Full task management (CRUD operations)
- **Users** - User information and management
- **Teams** - Team operations
- **Portfolios** - Portfolio management
- **Custom Fields** - Custom field operations
- **Attachments** - File uploads and downloads
- **Webhooks** - Webhook management
- **And much more...** - Complete API coverage

## Development

### Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download/dotnet/10.0)
- [Kiota](https://learn.microsoft.com/openapi/kiota/install) - `dotnet tool install --global Microsoft.OpenApi.Kiota`
- [Redocly CLI](https://redocly.com/docs/cli/) - `npm install -g @redocly/cli`

### Initial Setup

Run the initialization script to generate the API clients:

```bash
./init.sh
```

This will:
1. Download the latest official Asana OpenAPI specification from GitHub
2. Bundle and dereference the spec using Redocly
3. Clean the `Clients` directory
4. Generate fresh API clients with Kiota
5. Restore NuGet packages
6. Build the project

### Updating API Clients

When the Asana OpenAPI specification changes, update the clients:

```bash
./update.sh
```

This script automatically:
1. Downloads the latest official Asana OpenAPI spec from `https://raw.githubusercontent.com/Asana/openapi/master/defs/asana_oas.yaml`
2. Bundles and dereferences it with Redocly
3. Regenerates all API clients with Kiota
4. Restores packages and rebuilds

### Building

```bash
# Debug build
dotnet build

# Release build
dotnet build --configuration Release
```

### Creating NuGet Package

```bash
# Create package
dotnet pack --configuration Release

# Output will be in: bin/Release/Panda.NuGet.AsanaClient.{version}.nupkg
```

### Publishing to NuGet

Publishing is automated via GitHub Actions. See [`.github/RELEASE.md`](.github/RELEASE.md) for detailed instructions.

**Quick start:**

1. Set up `NUGET_API_KEY` secret in GitHub repository settings
2. Create and push a version tag:
   ```bash
   git tag v1.0.0
   git push origin v1.0.0
   ```
3. The GitHub Actions workflow will automatically:
   - Download latest Asana OpenAPI spec
   - Generate API clients
   - Build and test the project
   - Publish to NuGet.org
   - Create a GitHub release

**Manual publishing:**

```bash
# If needed, you can publish manually
dotnet nuget push ./nupkg/Panda.NuGet.AsanaClient.*.nupkg \
    --api-key YOUR_NUGET_API_KEY \
    --source https://api.nuget.org/v3/index.json
```

## Project Structure

```
Panda.NuGet.AsanaClient/
├── .github/
│   ├── workflows/
│   │   ├── build-and-test.yml       # CI workflow for builds
│   │   └── publish-nuget.yml        # CD workflow for releases
│   └── RELEASE.md                   # Release process documentation
├── init.sh                          # Initial setup script
├── update.sh                        # Update script for API clients
├── LICENSE                          # MIT License
├── README.md                        # This file
└── Panda.NuGet.AsanaClient/
    ├── Panda.NuGet.AsanaClient.csproj
    ├── AsanaApiClient.cs            # High-level API client wrapper
    ├── AsanaServiceCollectionExtensions.cs  # DI extensions
    ├── Extensions/                  # Partial class extensions for generated types
    │   └── TasksPostRequestBody_data.Extensions.cs  # Adds missing properties (Name, Notes, etc.)
    ├── asana_oas.yaml               # Downloaded original OpenAPI spec (not committed)
    ├── asana_flat.yaml              # Bundled/dereferenced spec (not committed)
    └── Clients/                     # ⚠️ Auto-generated - DO NOT EDIT
        ├── ApiClient.cs             # Kiota-generated base client
        ├── Tasks/                   # Generated task endpoints
        ├── Projects/                # Generated project endpoints
        └── ...                      # Other generated endpoints
```

**⚠️ Important:** The `Clients/` directory is completely auto-generated. Never manually edit files in this directory as they will be overwritten.

**💡 Tip:** The `Extensions/` directory contains partial class extensions that add missing properties to the auto-generated types (e.g., `Name`, `Notes` for tasks). These extensions are preserved during updates and provide a better developer experience.

## Getting an Asana Access Token

1. Log in to [Asana](https://app.asana.com)
2. Go to **Settings** → **Apps** → **Developer Apps**
3. Click **Create New Personal Access Token**
4. Give it a name and click **Create**
5. Copy the token immediately (you won't see it again)

## Authentication Best Practices

1. **Never hardcode tokens** - Use configuration and secrets management
2. **Use User Secrets** for local development
3. **Use Azure Key Vault, AWS Secrets Manager, or similar** for production
4. **Rotate tokens regularly**
5. **Use the minimum required permissions**

## Examples

### Create a Task

```csharp
var taskRequest = new TasksPostRequestBody
{
    Data = new TasksPostRequestBody_data
    {
        Name = "New Task",
        Notes = "Task description",
        Projects = new List<string> { "project-gid" },
        Assignee = "user-gid"
    }
};

var response = await asanaClient.Client.Tasks.PostAsync(taskRequest);
Console.WriteLine($"Created task: {response.Data.Gid}");
```

### Get Project Tasks

```csharp
var tasks = await asanaClient.Client.Tasks.GetAsync(config =>
{
    config.QueryParameters.Project = "project-gid";
    config.QueryParameters.Limit = 100;
});

foreach (var task in tasks.Data)
{
    Console.WriteLine($"Task: {task.Name} (Status: {task.Completed})");
}
```

### Update a Task

```csharp
var updateRequest = new TasksPutRequestBody
{
    Data = new TasksPutRequestBody_data
    {
        Completed = true,
        Notes = "Updated notes"
    }
};

await asanaClient.Client.Tasks["task-gid"].PutAsync(updateRequest);
```

## Troubleshooting

### 401 Unauthorized

- Verify your access token is correct
- Check that the token hasn't expired
- Ensure the token has the required permissions

### HttpClient Lifecycle Issues

- Always use `IHttpClientFactory` in production
- Don't manually dispose HttpClient when using DI
- The library is designed to work optimally with HttpClientFactory

### Generated Code Issues

If you encounter issues with generated code:

```bash
# Regenerate all clients
./init.sh
```

## Contributing

Contributions are welcome! Please:

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request

## License

This project is licensed under the MIT License.

Copyright (c) 2026 Mr. & Mrs. Panda - librics GmbH & Co. KG

## Resources

- [Asana API Documentation](https://developers.asana.com/docs)
- [Microsoft Kiota](https://learn.microsoft.com/openapi/kiota/)
- [HttpClientFactory Best Practices](https://learn.microsoft.com/dotnet/architecture/microservices/implement-resilient-applications/use-httpclientfactory-to-implement-resilient-http-requests)

## Support

For issues and questions:
- [GitHub Issues](https://github.com/mr-mrs-panda/asana-dotnet/issues)
- [Asana Developer Forum](https://forum.asana.com/c/developersapi)

---

**Note:** This library is not officially maintained by Asana and is provided as-is without any warranties or guarantees of future maintenance. For official support, please refer to the [Asana Developer Documentation](https://developers.asana.com/docs).
