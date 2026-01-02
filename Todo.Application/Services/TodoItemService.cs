namespace Todo.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoItemService(ITodoItemRepository todoItemRepository)
    {
        _todoItemRepository = todoItemRepository;
    }

    public async Task CreateAsync(TodoItem entity)
    {
        await _todoItemRepository.CreateAsync(entity);
    }

    public async Task DeleteAsync(int id)
    {
        await _todoItemRepository.DeleteAsync(new TodoItem() { TodoItemID = id });
    }

    public async Task<TodoItem> DetailsAsync(TodoItem entity)
    {
        var todoItem = await _todoItemRepository.GetAsync(entity);
        return todoItem;
    }

    public async Task<IEnumerable<TodoItem>> RetrieveAsync(int userID)
    {
        var todoItems = await _todoItemRepository.GetAllAsync(new TodoItem() { CreatedBy = new User() { UserID = userID } });
        return todoItems;
    }

    public async Task UpdateAsync(int id, TodoItem entity)
    {
        var todo = await _todoItemRepository.GetAsync(new TodoItem() { TodoItemID = id });

        if(todo is null)
            throw new Exception("Todo item not found.");

        todo.Name = entity.Name;
        todo.IsDone = entity.IsDone;
        todo.DeadLine = entity.DeadLine;

        await _todoItemRepository.UpdateAsync(todo);
    }
}
