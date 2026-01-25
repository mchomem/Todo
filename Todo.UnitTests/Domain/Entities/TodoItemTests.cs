namespace Todo.UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for the TodoItem entity
/// </summary>
public class TodoItemTests
{
    private readonly User _mockUser;

    public TodoItemTests()
    {
        _mockUser = new User("Test User", "testuser", "password123");
    }

    [Fact]
    public void Constructor_Should_Create_TodoItem_With_Valid_Data()
    {
        // Arrange
        var name = "Buy groceries";
        var deadline = DateTime.Now.AddDays(7);
        var userId = 1;

        // Act
        var todoItem = new TodoItem(name, deadline, userId, _mockUser);

        // Assert
        todoItem.Name.Should().Be(name);
        todoItem.DeadLine.Should().Be(deadline);
        todoItem.CreatedByID.Should().Be(userId);
        todoItem.IsDone.Should().BeFalse();
        todoItem.CreatedIn.Should().BeCloseTo(DateTime.Now, TimeSpan.FromSeconds(1));
    }

    [Fact]
    public void Constructor_Should_Create_TodoItem_Without_Deadline()
    {
        // Arrange
        var name = "Task without deadline";

        // Act
        var todoItem = new TodoItem(name, null, 1, _mockUser);

        // Assert
        todoItem.Name.Should().Be(name);
        todoItem.DeadLine.Should().BeNull();
        todoItem.IsDone.Should().BeFalse();
    }

    [Fact]
    public void Update_Should_Change_TodoItem_Properties()
    {
        // Arrange
        var todoItem = new TodoItem("Original Task", DateTime.Now.AddDays(5), 1, _mockUser);
        var newName = "Updated Task";
        var newDeadline = DateTime.Now.AddDays(10);

        // Act
        todoItem.Update(newName, true, newDeadline);

        // Assert
        todoItem.Name.Should().Be(newName);
        todoItem.IsDone.Should().BeTrue();
        todoItem.DeadLine.Should().Be(newDeadline);
    }

    [Fact]
    public void MaskAsDone_Should_Set_IsDone_To_True()
    {
        // Arrange
        var todoItem = new TodoItem("Task to complete", null, 1, _mockUser);
        todoItem.IsDone.Should().BeFalse();

        // Act
        todoItem.MaskAsDone();

        // Assert
        todoItem.IsDone.Should().BeTrue();
    }

    [Fact]
    public void Update_Should_Allow_Setting_IsDone_To_False()
    {
        // Arrange
        var todoItem = new TodoItem("Task", null, 1, _mockUser);
        todoItem.MaskAsDone();
        todoItem.IsDone.Should().BeTrue();

        // Act
        todoItem.Update("Task", false, null);

        // Assert
        todoItem.IsDone.Should().BeFalse();
    }
}
