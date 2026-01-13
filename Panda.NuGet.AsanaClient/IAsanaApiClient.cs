using Panda.NuGet.AsanaClient.Clients;

namespace Panda.NuGet.AsanaClient;

/// <summary>
/// Interface for the high-level Asana API client.
/// Use this interface for dependency injection and unit testing.
/// </summary>
public interface IAsanaApiClient
{
    /// <summary>
    /// Gets the underlying Kiota-generated API client for advanced scenarios.
    /// </summary>
    ApiClient Client { get; }
}
