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
    public required string Name { get; init; }
    public required bool IsDone { get; init; }
    public DateTime? DeadLine { get; set; }
    public required int CreatedByID { get; init; }
}

public class TodoItemUpdateDto
{
    public required int TodoItemID { get; init; }
    public required string Name { get; init; }
    public required bool IsDone { get; init; }
    public DateTime? DeadLine { get; set; }
}
