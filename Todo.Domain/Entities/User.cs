namespace Todo.Domain.Entities;

[Serializable]
public class User
{
    #region Propeties

    public int? UserID { get; set; }
    public string? Name { get; set; }
    public string? Login { get; set; }
    public string? Password { get; set; }
    public bool? IsActive { get; set; }

    [JsonIgnore]
    public ICollection<TodoItem>? TodoItems { get; set; }

    public UserPicture? Picture { get; set; }

    #endregion
}
