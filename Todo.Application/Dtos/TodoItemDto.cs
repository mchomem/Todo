namespace Todo.Application.Dtos;

public class TodoItemDto
{
    public int TodoItemID { get; set; }
    public string Name { get; set; }
    public bool IsDone { get; set; }
    public DateTime? DeadLine { get; set; }
    public int CreatedByID { get; set; }
    public UserDto CreatedBy { get; set; }
    public DateTime CreatedIn { get; set; }
}

public class TodoItemInsertDto
{
    public string Name { get; set; }
    public bool IsDone { get; set; }
    public DateTime? DeadLine { get; set; }
    public int CreatedByID { get; set; }
}

public class TodoItemUpdateDto
{
    public int TodoItemID { get; set; }
    public string Name { get; set; }
    public bool IsDone { get; set; }
    public DateTime? DeadLine { get; set; }
}
