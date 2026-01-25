namespace Todo.UnitTests.Domain.Entities;

/// <summary>
/// Unit tests for the User entity
/// </summary>
public class UserTests
{
    [Fact]
    public void Constructor_Should_Create_User_With_Valid_Data()
    {
        // Arrange
        var name = "John Doe";
        var login = "johndoe";
        var password = "securePassword123";

        // Act
        var user = new User(name, login, password);

        // Assert
        user.Name.Should().Be(name);
        user.Login.Should().Be(login);
        user.Password.Should().Be(password);
        user.IsActive.Should().BeTrue();
    }

    [Fact]
    public void Update_Should_Change_User_Name()
    {
        // Arrange
        var user = new User("Original Name", "login", "password");
        var newName = "Updated Name";

        // Act
        user.Update(newName);

        // Assert
        user.Name.Should().Be(newName);
        user.Login.Should().Be("login"); // Login should not change
    }

    [Fact]
    public void ChangePassword_Should_Update_Password()
    {
        // Arrange
        var user = new User("User", "login", "oldPassword");
        var newPassword = "newSecurePassword456";

        // Act
        user.ChangePassword(newPassword);

        // Assert
        user.Password.Should().Be(newPassword);
    }

    [Fact]
    public void User_Should_Have_Empty_TodoItems_Collection_Initially()
    {
        // Arrange & Act
        var user = new User("User", "login", "password");

        // Assert
        user.TodoItems.Should().BeNull();
    }
}
