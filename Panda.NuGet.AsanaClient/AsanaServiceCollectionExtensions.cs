using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Microsoft.Kiota.Http.HttpClientLibrary;
using Panda.NuGet.AsanaClient.Clients;

namespace Panda.NuGet.AsanaClient;

/// <summary>
/// Extension methods for configuring Asana API client services in an <see cref="IServiceCollection"/>.
/// </summary>
public static class AsanaServiceCollectionExtensions
{
    /// <summary>
    /// Adds the Asana API client to the service collection using a Personal Access Token.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="accessToken">Your Asana Personal Access Token</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAsanaClient(this IServiceCollection services, string accessToken)
    {
        if (string.IsNullOrWhiteSpace(accessToken))
        {
            throw new ArgumentException("Access token cannot be null or empty", nameof(accessToken));
        }

        services.AddHttpClient<AsanaApiClient>();

        services.AddSingleton<AsanaApiClient>(sp =>
        {
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(AsanaApiClient));
            return new AsanaApiClient(accessToken, httpClient);
        });

        return services;
    }

    /// <summary>
    /// Adds the Asana API client to the service collection using a token factory function.
    /// This is useful when the token needs to be retrieved from configuration or other sources.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="tokenFactory">A function that returns the access token</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAsanaClient(this IServiceCollection services, Func<IServiceProvider, string> tokenFactory)
    {
        if (tokenFactory == null)
        {
            throw new ArgumentNullException(nameof(tokenFactory));
        }

        services.AddHttpClient<AsanaApiClient>();

        services.AddSingleton<AsanaApiClient>(sp =>
        {
            var token = tokenFactory(sp);
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(AsanaApiClient));
            return new AsanaApiClient(token, httpClient);
        });

        return services;
    }

    /// <summary>
    /// Adds the Asana API client to the service collection with a custom authentication provider.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="authenticationProviderFactory">A function that creates the authentication provider</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAsanaClient(
        this IServiceCollection services,
        Func<IServiceProvider, IAuthenticationProvider> authenticationProviderFactory)
    {
        if (authenticationProviderFactory == null)
        {
            throw new ArgumentNullException(nameof(authenticationProviderFactory));
        }

        services.AddHttpClient<AsanaApiClient>();

        services.AddSingleton<AsanaApiClient>(sp =>
        {
            var authProvider = authenticationProviderFactory(sp);
            var httpClientFactory = sp.GetRequiredService<IHttpClientFactory>();
            var httpClient = httpClientFactory.CreateClient(nameof(AsanaApiClient));
            return new AsanaApiClient(authProvider, httpClient);
        });

        return services;
    }

    /// <summary>
    /// Adds both the AsanaApiClient and the underlying Kiota ApiClient to the service collection.
    /// This allows injecting either the high-level wrapper or the low-level generated client.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="accessToken">Your Asana Personal Access Token</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAsanaClientWithApiClient(this IServiceCollection services, string accessToken)
    {
        services.AddAsanaClient(accessToken);

        services.AddSingleton<ApiClient>(sp =>
        {
            var asanaClient = sp.GetRequiredService<AsanaApiClient>();
            return asanaClient.Client;
        });

        return services;
    }

    /// <summary>
    /// Adds both the AsanaApiClient and the underlying Kiota ApiClient to the service collection using a token factory.
    /// This allows injecting either the high-level wrapper or the low-level generated client.
    /// </summary>
    /// <param name="services">The service collection</param>
    /// <param name="tokenFactory">A function that returns the access token</param>
    /// <returns>The service collection for chaining</returns>
    public static IServiceCollection AddAsanaClientWithApiClient(
        this IServiceCollection services,
        Func<IServiceProvider, string> tokenFactory)
    {
        services.AddAsanaClient(tokenFactory);

        services.AddSingleton<ApiClient>(sp =>
        {
            var asanaClient = sp.GetRequiredService<AsanaApiClient>();
            return asanaClient.Client;
        });

        return services;
    }
}
