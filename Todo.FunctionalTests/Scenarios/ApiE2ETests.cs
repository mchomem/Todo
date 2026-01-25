namespace Todo.FunctionalTests.Scenarios;

/// <summary>
/// E2E functional tests focused on the API
/// Simulates complete user flow through the REST API
/// </summary>
public class ApiE2ETests : IClassFixture<TodoApiFactory>
{
    private readonly HttpClient _httpClient;

    public ApiE2ETests(TodoApiFactory factory)
    {
        _httpClient = factory.CreateClient();
    }

    [Fact]
    public async Task Complete_User_Journey_Through_API()
    {
        // This test simulates a complete user journey:
        // 1. Create account
        // 2. Authenticate
        // 3. Create tasks
        // 4. List tasks
        // 5. Mark as completed
        // 6. Delete task

        var username = $"apitest_{Guid.NewGuid():N}".Substring(0, 19);
        var password = "ApiTest123";

        // 1. Create user
        var newUser = new
        {
            Name = "API E2E Test User",
            Login = username,
            Password = password,
            PasswordConfirmation = password
        };

        var createUserResponse = await _httpClient.PostAsJsonAsync("/api/user", newUser);
        createUserResponse.EnsureSuccessStatusCode();

        // 2. Authenticate
        var authResponse = await _httpClient.GetAsync($"/api/auth/authentication?login={username}&password={password}");
        authResponse.EnsureSuccessStatusCode();

        var authResult = await authResponse.Content.ReadFromJsonAsync<ApiResponse>();
        var token = authResult?.Data?.GetProperty("token").GetString();
        token.Should().NotBeNullOrEmpty();

        // Configure token for next requests
        _httpClient.DefaultRequestHeaders.Authorization = 
            new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

        // 3. Create tasks
        var tasks = new[]
        {
            new { Name = "Task 1 - E2E", IsDone = false, CreatedByID = 1 },
            new { Name = "Task 2 - E2E", IsDone = false, CreatedByID = 1 },
            new { Name = "Task 3 - E2E", IsDone = false, CreatedByID = 1 }
        };

        foreach (var task in tasks)
        {
            var response = await _httpClient.PostAsJsonAsync("/api/todoitem", task);
            response.EnsureSuccessStatusCode();
        }

        // 4. List tasks
        var listResponse = await _httpClient.GetAsync("/api/todoitem?userID=1");
        listResponse.EnsureSuccessStatusCode();

        var todoList = await listResponse.Content.ReadFromJsonAsync<ApiResponse>();
        todoList.Should().NotBeNull();

        // 5. Mark first task as completed
        var completeResponse = await _httpClient.PutAsync("/api/todoitem/complete/1", null);
        completeResponse.IsSuccessStatusCode.Should().BeTrue();

        // 6. Update second task
        var updateTask = new
        {
            TodoItemID = 2,
            Name = "Task 2 - Updated",
            IsDone = true,
            DeadLine = DateTime.Now.AddDays(7)
        };

        var updateResponse = await _httpClient.PutAsJsonAsync("/api/todoitem/2", updateTask);
        updateResponse.IsSuccessStatusCode.Should().BeTrue();

        // 7. Delete third task
        var deleteResponse = await _httpClient.DeleteAsync("/api/todoitem/3");
        deleteResponse.IsSuccessStatusCode.Should().BeTrue();
    }

    [Fact]
    public async Task Should_Enforce_Authentication_On_Protected_Endpoints()
    {
        // Tests if protected endpoints require authentication

        // Act - Try to access protected endpoint without token
        var response = await _httpClient.GetAsync("/api/todoitem?userID=1");

        // Assert
        response.StatusCode.Should().Be(System.Net.HttpStatusCode.Unauthorized);
    }

    // Helper class to deserialize API response
    private class ApiResponse
    {
        public System.Text.Json.JsonElement? Data { get; set; }
        public bool Success { get; set; }
    }
}
