namespace Todo.Domain.Entities;

[Serializable]
public class User
{
    private User() { }

    public User(string name, string login, string password)
    {
        Name = name;
        Login = login;
        Password = password;
        IsActive = true;
    }

    public int UserID { get; private set; }
    public string Name { get; private set; }
    public string Login { get; private set; }
    public string Password { get; private set; }
    public bool IsActive { get; private set; }

    [JsonIgnore]
    public ICollection<TodoItem>? TodoItems { get; private set; }

    public UserPicture? Picture { get; set; }

    public void Update(string name)
    {
        Name = name;
    }

    public void ChangePassword(string password)
    {
        Password = password;
    }
}
