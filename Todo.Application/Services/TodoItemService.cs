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

    public async Task UpdateAsync(TodoItem entity)
    {
        await _todoItemRepository.UpdateAsync(entity);
    }
}
