namespace Todo.Domain.Exceptions.TodoItem
{
    public class TodoItemException : BusinessException
    {
        public TodoItemException(string message) : base(message) { }
    }
}
