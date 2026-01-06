namespace Todo.Application.Dtos;

public class UserDto
{
    public int UserID { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public byte[]? Picture { get; set; }

    public string? Token { get; set; }
}

public class UserInsertDto
{
    public string Name { get; set; } = string.Empty;
    public string Login { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

public class UserUpdateDto
{
    public int UserID { get; set; }
    public string? Name { get; set; }
    public bool IsActive { get; set; }
    public byte[]? Picture { get; set; }
}

public class  UserChangePasswordDto
{
    public int UserID { get; set; }
    public string CurrentPassword { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}