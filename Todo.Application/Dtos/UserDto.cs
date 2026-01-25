namespace Todo.Application.Dtos;

public class UserDto
{
    public int UserID { get; set; }
    public string? Name { get; set; }
    public string? Login { get; set; }
    public bool IsActive { get; set; }
    public byte[]? Picture { get; set; }
    public string? Token { get; set; }
}

public class UserInsertDto
{
    public required string Name { get; init; }
    public required string Login { get; init; }
    public required string Password { get; init; }
}

public class UserUpdateDto
{
    public required int UserID { get; init; }
    public required string Name { get; init; }
    public bool IsActive { get; set; }
    public byte[]? Picture { get; set; }
}

public class  UserChangePasswordDto
{
    public required int UserID { get; init; }
    public required string CurrentPassword { get; init; }
    public required string NewPassword { get; init; }
}
