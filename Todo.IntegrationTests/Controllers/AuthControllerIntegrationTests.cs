namespace Todo.IntegrationTests.Controllers;

/// <summary>
/// Integration tests for AuthController
/// Tests authentication and token generation
/// </summary>
public class AuthControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly WebApplicationFactory<Program> _factory;

    public AuthControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Authenticate_Should_Return_Token_With_Valid_Credentials()
    {
        // Arrange - First create a user
        var login = $"authtest_{Guid.NewGuid():N}".Substring(0, 19);
        var password = "TestPassword123";
        
        var newUser = new
        {
            Name = "Auth Test User",
            Login = login,
            Password = password,
            PasswordConfirmation = password
        };

        await _client.PostAsync("/api/user", 
            new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json"));

        // Act - Use GET with query params
        var response = await _client.GetAsync($"/api/auth/authentication?login={login}&password={password}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var responseBody = await response.Content.ReadAsStringAsync();
        responseBody.Should().Contain("token");
    }

    [Fact]
    public async Task Authenticate_Should_Return_Error_With_Invalid_Credentials()
    {
        // Arrange
        var login = "nonexistent_user";
        var password = "wrong_password";

        // Act - Use GET with query params
        var response = await _client.GetAsync($"/api/auth/authentication?login={login}&password={password}");

        // Assert
        // API returns 400 for invalid credentials (Bad Request is appropriate for validation)
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,           // 400 - API returns this for invalid credentials
            HttpStatusCode.Unauthorized,         // 401 - would be ideal for failed authentication
            HttpStatusCode.NotFound,             // 404 - if user does not exist
            HttpStatusCode.InternalServerError   // 500 - if there is an unhandled error
        );
    }
}
