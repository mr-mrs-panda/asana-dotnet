using FluentAssertions;
using Microsoft.Kiota.Abstractions;
using Microsoft.Kiota.Abstractions.Authentication;
using Moq;

namespace Panda.NuGet.AsanaClient.Tests;

public class AsanaApiClientTests
{
    private readonly HttpClient _httpClient;

    public AsanaApiClientTests()
    {
        _httpClient = new HttpClient();
    }

    [Fact]
    public void Constructor_WithAccessToken_ShouldCreateClient()
    {
        // Arrange
        const string accessToken = "test-token-12345";

        // Act
        var client = new AsanaApiClient(accessToken, _httpClient);

        // Assert
        client.Should().NotBeNull();
        client.Client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullAccessToken_ShouldThrowArgumentException()
    {
        // Arrange
        string? accessToken = null;

        // Act
        Action act = () => new AsanaApiClient(accessToken!, _httpClient);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("accessToken")
            .WithMessage("Access token cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithEmptyAccessToken_ShouldThrowArgumentException()
    {
        // Arrange
        const string accessToken = "";

        // Act
        Action act = () => new AsanaApiClient(accessToken, _httpClient);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("accessToken")
            .WithMessage("Access token cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithWhitespaceAccessToken_ShouldThrowArgumentException()
    {
        // Arrange
        const string accessToken = "   ";

        // Act
        Action act = () => new AsanaApiClient(accessToken, _httpClient);

        // Assert
        act.Should().Throw<ArgumentException>()
            .WithParameterName("accessToken")
            .WithMessage("Access token cannot be null or empty*");
    }

    [Fact]
    public void Constructor_WithNullHttpClient_ShouldThrowArgumentNullException()
    {
        // Arrange
        const string accessToken = "test-token";
        HttpClient? httpClient = null;

        // Act
        Action act = () => new AsanaApiClient(accessToken, httpClient!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("httpClient");
    }

    [Fact]
    public void Constructor_WithAuthenticationProvider_ShouldCreateClient()
    {
        // Arrange
        var mockAuthProvider = new Mock<IAuthenticationProvider>();

        // Act
        var client = new AsanaApiClient(mockAuthProvider.Object, _httpClient);

        // Assert
        client.Should().NotBeNull();
        client.Client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullAuthenticationProvider_ShouldThrowArgumentNullException()
    {
        // Arrange
        IAuthenticationProvider? authProvider = null;

        // Act
        Action act = () => new AsanaApiClient(authProvider!, _httpClient);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("authenticationProvider");
    }

    [Fact]
    public void Constructor_WithAuthenticationProviderAndNullHttpClient_ShouldThrowArgumentNullException()
    {
        // Arrange
        var mockAuthProvider = new Mock<IAuthenticationProvider>();
        HttpClient? httpClient = null;

        // Act
        Action act = () => new AsanaApiClient(mockAuthProvider.Object, httpClient!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("httpClient");
    }

    [Fact]
    public void Constructor_WithRequestAdapter_ShouldCreateClient()
    {
        // Arrange
        var mockRequestAdapter = new Mock<IRequestAdapter>();

        // Act
        var client = new AsanaApiClient(mockRequestAdapter.Object);

        // Assert
        client.Should().NotBeNull();
        client.Client.Should().NotBeNull();
    }

    [Fact]
    public void Constructor_WithNullRequestAdapter_ShouldThrowArgumentNullException()
    {
        // Arrange
        IRequestAdapter? requestAdapter = null;

        // Act
        Action act = () => new AsanaApiClient(requestAdapter!);

        // Assert
        act.Should().Throw<ArgumentNullException>()
            .WithParameterName("requestAdapter");
    }

    [Fact]
    public void Client_Property_ShouldReturnUnderlyingApiClient()
    {
        // Arrange
        const string accessToken = "test-token";
        var client = new AsanaApiClient(accessToken, _httpClient);

        // Act
        var apiClient = client.Client;

        // Assert
        apiClient.Should().NotBeNull();
    }

    [Fact]
    public void Client_Property_ShouldReturnSameInstanceOnMultipleCalls()
    {
        // Arrange
        const string accessToken = "test-token";
        var client = new AsanaApiClient(accessToken, _httpClient);

        // Act
        var apiClient1 = client.Client;
        var apiClient2 = client.Client;

        // Assert
        apiClient1.Should().BeSameAs(apiClient2);
    }
}
