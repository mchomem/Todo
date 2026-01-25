namespace Todo.IntegrationTests.Controllers;

/// <summary>
/// Integration tests for TodoItemController
/// Tests the API endpoints related to task items
/// </summary>
public class TodoItemControllerIntegrationTests : BaseIntegrationTest
{
    public TodoItemControllerIntegrationTests(WebApplicationFactory<Program> factory) : base(factory) { }

    [Fact]
    public async Task PostAsync_Should_Create_TodoItem_With_Valid_Token()
    {
        // Arrange
        var (token, _, userId) = await CreateUserAndGetTokenAsync();
        SetAuthorizationHeader(token);

        var newTodo = new
        {
            Name = "Test Task",
            IsDone = false,
            DeadLine = DateTime.Now.AddDays(7),
            CreatedByID = userId
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newTodo),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await Client.PostAsync("/api/todoitem", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task PostAsync_Should_Return_Unauthorized_Without_Token()
    {
        // Arrange
        ClearAuthorizationHeader();

        var newTodo = new
        {
            Name = "Unauthorized Task",
            IsDone = false,
            CreatedByID = 999  // Fictional ID since the request should not be processed
        };

        var content = new StringContent(
            JsonSerializer.Serialize(newTodo),
            Encoding.UTF8,
            "application/json"
        );

        // Act
        var response = await Client.PostAsync("/api/todoitem", content);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }

    [Fact]
    public async Task GetAsync_Should_Return_TodoItems_For_User()
    {
        // Arrange
        var (token, _, userId) = await CreateUserAndGetTokenAsync();
        SetAuthorizationHeader(token);

        // Act
        var response = await Client.GetAsync($"/api/todoitem?userID={userId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    [Fact]
    public async Task PutMarkTaskAsCompleteAsync_Should_Mark_Task_As_Complete()
    {
        // Arrange
        var (token, _, userId) = await CreateUserAndGetTokenAsync();
        SetAuthorizationHeader(token);

        // Criar uma tarefa primeiro
        var newTodo = new
        {
            Name = "Task to Complete",
            IsDone = false,
            CreatedByID = userId
        };

        var createResponse = await Client.PostAsync("/api/todoitem",
            new StringContent(JsonSerializer.Serialize(newTodo), Encoding.UTF8, "application/json"));
        
        createResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        // Fetch all items to get the ID (the API does not return the created object)
        var getResponse = await Client.GetAsync($"/api/todoitem?userID={userId}");
        var getContent = await getResponse.Content.ReadAsStringAsync();
        var responseData = JsonSerializer.Deserialize<JsonElement>(getContent);
        var dataArray = responseData.GetProperty("data");
        
        // Get the first item (the one we just created)
        var firstItem = dataArray.EnumerateArray().FirstOrDefault();
        var todoId = firstItem.GetProperty("todoItemID").GetInt32();

        // Act
        var response = await Client.PutAsync($"/api/todoitem/complete/{todoId}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }
}
