namespace Todo.Domain.Dtos;

[Serializable]
public class UserDto
{
    #region Propeties

    public int UserID { get; set; }

    public string? Name { get; set; }

    public bool IsActive { get; set; }

    public byte[]? Picture { get; set; }

    public string? Token { get; set; }

    #endregion
}
