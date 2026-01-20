namespace Todo.Domain.Exceptions.TodoItem;

public class TodoItemNotFoundException : TodoItemException
{
    public TodoItemNotFoundException(string message = DefaultMessages.TodoItemNotFound) : base(message) { }
}
