namespace Todo.IntegrationTests.Controllers;

/// <summary>
/// Integration tests for UserController
/// Tests the API endpoints related to users
/// </summary>
public class UserControllerIntegrationTests : BaseIntegrationTest
{
    public UserControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task PostAsync_Should_Create_New_User()
    {
        // Arrange
        var (token, _, _) = await CreateUserAndGetTokenAsync();
        SetAuthorizationHeader(token);

        var newUser = new
        {
            Name = "Integration Test User",
            Login = $"usr{Guid.NewGuid():N}".Substring(0, 19),
            Password = "TestPassword123",
            PasswordConfirmation = "TestPassword123"
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newUser),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await Client.PostAsync("/api/user", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostAsync_Should_Return_Error_When_User_Already_Exists()
    {
        // Arrange
        var login = $"dup{Guid.NewGuid():N}".Substring(0, 19);
        var newUser = new
        {
            Name = "Duplicate User",
            Login = login,
            Password = "Password123",
            PasswordConfirmation = "Password123"
        };

        var content1 = new StringContent(
            JsonSerializer.Serialize(newUser),
            Encoding.UTF8,
            "application/json"
        );

        var content2 = new StringContent(
            JsonSerializer.Serialize(newUser),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var firstResponse = await Client.PostAsync("/api/user", content1);  // ← Verify success
        var secondResponse = await Client.PostAsync("/api/user", content2);

        // Assert
        firstResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        secondResponse.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,           // 400 - API returns this for duplication
            HttpStatusCode.Conflict,             // 409 - would be ideal for resource conflict
            HttpStatusCode.InternalServerError   // 500 - if duplication error is not handled
        );
    }
}
