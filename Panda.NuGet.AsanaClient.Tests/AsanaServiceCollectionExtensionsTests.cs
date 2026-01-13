using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Kiota.Abstractions.Authentication;
using Moq;
using Panda.NuGet.AsanaClient.Clients;

namespace Panda.NuGet.AsanaClient.Tests;

public class AsanaServiceCollectionExtensionsTests
{
    [Fact]
    public void AddAsanaClient_WithAccessToken_ShouldRegisterAsanaApiClient()
    {
        // Arrange
        var services = new ServiceCollection();
        const string accessToken = "test-token-12345";

        // Act
        services.AddAsanaClient(accessToken);
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<AsanaApiClient>();

        // Assert
        client.Should().NotBeNull();
        client!.Client.Should().NotBeNull();
    }

    [Fact]
    public void AddAsanaClient_WithAccessToken_ShouldRegisterAsSingleton()
    {
        // Arrange
        var services = new ServiceCollection();
        const string accessToken = "test-token";

        // Act
        services.AddAsanaClient(accessToken);
        var serviceProvider = services.BuildServiceProvider();
        var client1 = serviceProvider.GetService<AsanaApiClient>();
        var client2 = serviceProvider.GetService<AsanaApiClient>();

        // Assert
        client1.Should().BeSameAs(client2);
    }

    [Fact]
    public void AddAsanaClient_WithNullAccessToken_ShouldThrowArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();
        string? accessToken = null;

        // Act
        Action act = () => services.AddAsanaClient(accessToken!);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("accessToken")
            .WithMessage("Access token cannot be null or empty*");
    }

    [Fact]
    public void AddAsanaClient_WithEmptyAccessToken_ShouldThrowArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();
        const string accessToken = "";

        // Act
        Action act = () => services.AddAsanaClient(accessToken);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("accessToken")
            .WithMessage("Access token cannot be null or empty*");
    }

    [Fact]
    public void AddAsanaClient_WithWhitespaceAccessToken_ShouldThrowArgumentException()
    {
        // Arrange
        var services = new ServiceCollection();
        const string accessToken = "   ";

        // Act
        Action act = () => services.AddAsanaClient(accessToken);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("accessToken")
            .WithMessage("Access token cannot be null or empty*");
    }

    [Fact]
    public void AddAsanaClient_WithTokenFactory_ShouldRegisterAsanaApiClient()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<IServiceProvider, string> tokenFactory = _ => "factory-token";

        // Act
        services.AddAsanaClient(tokenFactory);
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<AsanaApiClient>();

        // Assert
        client.Should().NotBeNull();
        client!.Client.Should().NotBeNull();
    }

    [Fact]
    public void AddAsanaClient_WithTokenFactory_ShouldCallFactoryFunction()
    {
        // Arrange
        var services = new ServiceCollection();
        var factoryCalled = false;
        Func<IServiceProvider, string> tokenFactory = _ =>
        {
            factoryCalled = true;
            return "factory-token";
        };

        // Act
        services.AddAsanaClient(tokenFactory);
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<AsanaApiClient>();

        // Assert
        factoryCalled.Should().BeTrue();
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddAsanaClient_WithNullTokenFactory_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<IServiceProvider, string>? tokenFactory = null;

        // Act
        Action act = () => services.AddAsanaClient(tokenFactory!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("tokenFactory");
    }

    [Fact]
    public void AddAsanaClient_WithAuthenticationProviderFactory_ShouldRegisterAsanaApiClient()
    {
        // Arrange
        var services = new ServiceCollection();
        var mockAuthProvider = new Mock<IAuthenticationProvider>();
        Func<IServiceProvider, IAuthenticationProvider> authProviderFactory = _ => mockAuthProvider.Object;

        // Act
        services.AddAsanaClient(authProviderFactory);
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<AsanaApiClient>();

        // Assert
        client.Should().NotBeNull();
        client!.Client.Should().NotBeNull();
    }

    [Fact]
    public void AddAsanaClient_WithAuthenticationProviderFactory_ShouldCallFactoryFunction()
    {
        // Arrange
        var services = new ServiceCollection();
        var factoryCalled = false;
        var mockAuthProvider = new Mock<IAuthenticationProvider>();
        Func<IServiceProvider, IAuthenticationProvider> authProviderFactory = _ =>
        {
            factoryCalled = true;
            return mockAuthProvider.Object;
        };

        // Act
        services.AddAsanaClient(authProviderFactory);
        var serviceProvider = services.BuildServiceProvider();
        var client = serviceProvider.GetService<AsanaApiClient>();

        // Assert
        factoryCalled.Should().BeTrue();
        client.Should().NotBeNull();
    }

    [Fact]
    public void AddAsanaClient_WithNullAuthenticationProviderFactory_ShouldThrowArgumentNullException()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<IServiceProvider, IAuthenticationProvider>? authProviderFactory = null;

        // Act
        Action act = () => services.AddAsanaClient(authProviderFactory!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("authenticationProviderFactory");
    }

    [Fact]
    public void AddAsanaClient_ShouldReturnServiceCollection_ForChaining()
    {
        // Arrange
        var services = new ServiceCollection();
        const string accessToken = "test-token";

        // Act
        var result = services.AddAsanaClient(accessToken);

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddAsanaClientWithApiClient_WithAccessToken_ShouldRegisterBothClients()
    {
        // Arrange
        var services = new ServiceCollection();
        const string accessToken = "test-token";

        // Act
        services.AddAsanaClientWithApiClient(accessToken);
        var serviceProvider = services.BuildServiceProvider();
        var asanaClient = serviceProvider.GetService<AsanaApiClient>();
        var apiClient = serviceProvider.GetService<ApiClient>();

        // Assert
        asanaClient.Should().NotBeNull();
        apiClient.Should().NotBeNull();
        apiClient.Should().BeSameAs(asanaClient!.Client);
    }

    [Fact]
    public void AddAsanaClientWithApiClient_WithTokenFactory_ShouldRegisterBothClients()
    {
        // Arrange
        var services = new ServiceCollection();
        Func<IServiceProvider, string> tokenFactory = _ => "factory-token";

        // Act
        services.AddAsanaClientWithApiClient(tokenFactory);
        var serviceProvider = services.BuildServiceProvider();
        var asanaClient = serviceProvider.GetService<AsanaApiClient>();
        var apiClient = serviceProvider.GetService<ApiClient>();

        // Assert
        asanaClient.Should().NotBeNull();
        apiClient.Should().NotBeNull();
        apiClient.Should().BeSameAs(asanaClient!.Client);
    }

    [Fact]
    public void AddAsanaClientWithApiClient_ShouldReturnServiceCollection_ForChaining()
    {
        // Arrange
        var services = new ServiceCollection();
        const string accessToken = "test-token";

        // Act
        var result = services.AddAsanaClientWithApiClient(accessToken);

        // Assert
        result.Should().BeSameAs(services);
    }

    [Fact]
    public void AddAsanaClient_ShouldRegisterHttpClient()
    {
        // Arrange
        var services = new ServiceCollection();
        const string accessToken = "test-token";

        // Act
        services.AddAsanaClient(accessToken);
        var serviceProvider = services.BuildServiceProvider();
        var httpClientFactory = serviceProvider.GetService<IHttpClientFactory>();

        // Assert
        httpClientFactory.Should().NotBeNull();
    }
}
