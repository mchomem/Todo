namespace Todo.UnitTests.Application.Services;

/// <summary>
/// Unit tests for the TodoItemService service
/// </summary>
public class TodoItemServiceTests
{
    private readonly Mock<ITodoItemRepository> _todoItemRepositoryMock;
    private readonly Mock<IUserRepository> _userRepositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly TodoItemService _service;

    public TodoItemServiceTests()
    {
        _todoItemRepositoryMock = new Mock<ITodoItemRepository>();
        _userRepositoryMock = new Mock<IUserRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new TodoItemService(
            _todoItemRepositoryMock.Object,
            _userRepositoryMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Create_TodoItem_When_User_Exists()
    {
        // Arrange
        var user = new User("John Doe", "johndoe", "password");
        var todoItemDto = new TodoItemInsertDto
        {
            Name = "Buy groceries",
            IsDone = false,
            DeadLine = DateTime.Now.AddDays(7),
            CreatedByID = 1
        };

        _userRepositoryMock
            .Setup(x => x.GetAsync(todoItemDto.CreatedByID))
            .ReturnsAsync(user);

        // Act
        await _service.CreateAsync(todoItemDto);

        // Assert
        _todoItemRepositoryMock.Verify(
            x => x.CreateAsync(It.Is<TodoItem>(t =>
                t.Name == todoItemDto.Name &&
                t.CreatedByID == todoItemDto.CreatedByID
            )),
            Times.Once
        );
    }

    [Fact]
    public async Task CreateAsync_Should_Throw_UserNotFoundException_When_User_Does_Not_Exist()
    {
        // Arrange
        var todoItemDto = new TodoItemInsertDto
        {
            Name = "Test Task",
            IsDone = false,
            CreatedByID = 999
        };

        _userRepositoryMock
            .Setup(x => x.GetAsync(todoItemDto.CreatedByID))
            .ReturnsAsync((User)null!);

        // Act & Assert
        await Assert.ThrowsAsync<UserNotFoundException>(
            () => _service.CreateAsync(todoItemDto)
        );
    }

    [Fact]
    public async Task GetAsync_Should_Return_TodoItem_When_Exists()
    {
        // Arrange
        var user = new User("User", "login", "password");
        var todoItem = new TodoItem("Task", DateTime.Now.AddDays(1), 1, user);
        var expectedDto = new TodoItemDto
        {
            TodoItemID = 1,
            Name = "Task",
            IsDone = false
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(todoItem);

        _mapperMock
            .Setup(x => x.Map<TodoItemDto>(todoItem))
            .Returns(expectedDto);

        // Act
        var result = await _service.GetAsync(1);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(expectedDto.Name);
    }

    [Fact]
    public async Task GetAsync_Should_Throw_TodoItemNotFoundException_When_Not_Exists()
    {
        // Arrange
        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(It.IsAny<int>()))
            .ReturnsAsync((TodoItem)null!);

        // Act & Assert
        await Assert.ThrowsAsync<TodoItemNotFoundException>(
            () => _service.GetAsync(999)
        );
    }

    [Fact]
    public async Task DeleteAsync_Should_Delete_TodoItem_When_Exists()
    {
        // Arrange
        var user = new User("User", "login", "password");
        var todoItem = new TodoItem("Task", null, 1, user);

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(todoItem);

        // Act
        await _service.DeleteAsync(1);

        // Assert
        _todoItemRepositoryMock.Verify(
            x => x.DeleteAsync(todoItem),
            Times.Once
        );
    }

    [Fact]
    public async Task DeleteAsync_Should_Throw_TodoItemNotFoundException_When_Not_Exists()
    {
        // Arrange
        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(It.IsAny<int>()))
            .ReturnsAsync((TodoItem)null!);

        // Act & Assert
        await Assert.ThrowsAsync<TodoItemNotFoundException>(
            () => _service.DeleteAsync(999)
        );
    }

    [Fact]
    public async Task MarkTaskAsComplete_Should_Mark_TodoItem_As_Done()
    {
        // Arrange
        var user = new User("User", "login", "password");
        var todoItem = new TodoItem("Task", null, 1, user);

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(todoItem);

        // Act
        await _service.MarkTaskAsComplete(1);

        // Assert
        todoItem.IsDone.Should().BeTrue();
        _todoItemRepositoryMock.Verify(
            x => x.UpdateAsync(todoItem),
            Times.Once
        );
    }

    [Fact]
    public async Task UpdateAsync_Should_Update_TodoItem_Properties()
    {
        // Arrange
        var user = new User("User", "login", "password");
        var todoItem = new TodoItem("Original Task", null, 1, user);
        var updateDto = new TodoItemUpdateDto
        {
            TodoItemID = 1,
            Name = "Updated Task",
            IsDone = true,
            DeadLine = DateTime.Now.AddDays(5)
        };

        _todoItemRepositoryMock
            .Setup(x => x.GetAsync(1))
            .ReturnsAsync(todoItem);

        // Act
        await _service.UpdateAsync(1, updateDto);

        // Assert
        todoItem.Name.Should().Be(updateDto.Name);
        todoItem.IsDone.Should().Be(updateDto.IsDone);
        _todoItemRepositoryMock.Verify(
            x => x.UpdateAsync(todoItem),
            Times.Once
        );
    }
}
