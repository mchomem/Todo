namespace Todo.Domain.Entities;

[Serializable]
public class TodoItem
{
    private TodoItem() { }

    public TodoItem(string name, DateTime? deadLine, int createdByID, User createdBy)
    {
        Name = name;
        IsDone = false;
        DeadLine = deadLine;
        CreatedByID = createdByID;
        CreatedBy = createdBy;
        CreatedIn = DateTime.Now;
    }

    public int TodoItemID { get; private set; }
    public string Name { get; private set; }
    public bool IsDone { get; private set; }
    public DateTime? DeadLine { get; private set; }
    public int CreatedByID { get; private set; }
    public User CreatedBy { get; private set; }
    public DateTime CreatedIn { get; private set; }

    public void Update(string name, bool isDone, DateTime? deadLine)
    {
        Name = name;
        IsDone = isDone;
        DeadLine = deadLine;
    }

    public void MaskAsDone()
    {
        IsDone = true;
    }
}
