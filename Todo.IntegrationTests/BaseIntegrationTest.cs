namespace Todo.IntegrationTests;

/// <summary>
/// Base class for integration tests
/// Provides common functionalities such as authentication and HTTP client configuration
/// </summary>
public abstract class BaseIntegrationTest : IClassFixture<WebApplicationFactory<Program>>
{
    protected readonly HttpClient Client;
    protected readonly WebApplicationFactory<Program> Factory;
    protected readonly ITokenService TokenService;

    protected BaseIntegrationTest(WebApplicationFactory<Program> factory)
    {
        Factory = factory;
        Client = factory.CreateClient();

        // Get the TokenService from the DI container
        var scope = Factory.Services.CreateScope();
        TokenService = scope.ServiceProvider.GetRequiredService<ITokenService>();
    }

    /// <summary>
    /// Creates a new user and returns the authentication token
    /// </summary>
    /// <returns>Tuple containing the JWT token, login, and UserID of the created user</returns>
    protected async Task<(string token, string login, int userId)> CreateUserAndGetTokenAsync()
    {
        var login = $"testuser_{Guid.NewGuid():N}".Substring(0, 19);
        var password = "TestPassword123";

        var newUser = new
        {
            Name = "Integration Test User",
            Login = login,
            Password = password,
            PasswordConfirmation = password
        };

        var createResponse = await Client.PostAsync("/api/user",
            new StringContent(JsonSerializer.Serialize(newUser), Encoding.UTF8, "application/json"));
        
        createResponse.EnsureSuccessStatusCode();

        // Authenticate using GET /api/auth/authentication?login=...&password=...
        var authResponse = await Client.GetAsync($"/api/auth/authentication?login={login}&password={password}");
        
        var authContent = await authResponse.Content.ReadAsStringAsync();
        var authData = JsonSerializer.Deserialize<JsonElement>(authContent);
        
        // Access Data.userID (the API returns ApiResponse<UserDto>)
        var userData = authData.GetProperty("data");
        var userId = userData.GetProperty("userID").GetInt32();
        var token = userData.GetProperty("token").GetString() ?? "";

        return (token, login, userId);
    }

    /// <summary>
    /// Configures the HTTP client authentication header
    /// </summary>
    /// <param name="token">JWT token for authentication</param>
    protected void SetAuthorizationHeader(string token)
    {
        Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>
    /// Clears the HTTP client authentication header
    /// </summary>
    protected void ClearAuthorizationHeader()
    {
        Client.DefaultRequestHeaders.Authorization = null;
    }
}
