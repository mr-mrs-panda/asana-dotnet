using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Panda.NuGet.AsanaClient.Clients;

namespace Panda.NuGet.AsanaClient;

/// <summary>
/// High-level client for interacting with the Asana API.
/// Wraps the auto-generated Kiota API client with a simplified interface.
/// </summary>
public class AsanaApiClient : IAsanaApiClient
{
    private readonly ApiClient _apiClient;

    /// <summary>
    /// Gets the underlying Kiota-generated API client for advanced scenarios.
    /// </summary>
    public ApiClient Client => _apiClient;

    /// <summary>
    /// Initializes a new instance of the AsanaApiClient with a Personal Access Token.
    /// </summary>
    /// <param name="accessToken">Your Asana Personal Access Token</param>
    /// <param name="httpClient">HttpClient instance. When using DI, this should come from IHttpClientFactory.</param>
    public AsanaApiClient(string accessToken, HttpClient httpClient)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
        }

        ArgumentNullException.ThrowIfNull(httpClient);

        var authProvider = new BaseBearerTokenAuthenticationProvider(new TokenProvider(accessToken));
        var adapter = new HttpClientRequestAdapter(authProvider, httpClient: httpClient);

        _apiClient = new ApiClient(adapter);
    }

    /// <summary>
    /// Initializes a new instance of the AsanaApiClient with a custom authentication provider.
    /// </summary>
    /// <param name="authenticationProvider">Custom authentication provider</param>
    /// <param name="httpClient">HttpClient instance. When using DI, this should come from IHttpClientFactory.</param>
    public AsanaApiClient(IAuthenticationProvider authenticationProvider, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(authenticationProvider);
        ArgumentNullException.ThrowIfNull(httpClient);

        var adapter = new HttpClientRequestAdapter(authenticationProvider, httpClient: httpClient);

        _apiClient = new ApiClient(adapter);
    }

    /// <summary>
    /// Initializes a new instance of the AsanaApiClient with a custom request adapter.
    /// For advanced scenarios where you need full control over the request adapter.
    /// </summary>
    /// <param name="requestAdapter">Custom request adapter</param>
    public AsanaApiClient(IRequestAdapter requestAdapter)
    {
        ArgumentNullException.ThrowIfNull(requestAdapter);

        _apiClient = new ApiClient(requestAdapter);
    }

    /// <summary>
    /// Simple token provider for Bearer token authentication.
    /// </summary>
    private class TokenProvider : IAccessTokenProvider
    {
        private readonly string _token;

        public TokenProvider(string token)
        {
            _token = token;
        }

        public Task<string> GetAuthorizationTokenAsync(Uri uri, Dictionary<string, object>? additionalAuthenticationContext = null, CancellationToken cancellationToken = default)
        {
            return Task.FromResult(_token);
        }

        public AllowedHostsValidator AllowedHostsValidator => new AllowedHostsValidator();
    }
}
