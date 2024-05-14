namespace Todo.Application.Services;

public class TodoItemService : ITodoItemService
{
    private readonly ITodoItemRepository _todoItemRepository;

    public TodoItemService(ITodoItemRepository todoItemRepository)
        => _todoItemRepository = todoItemRepository;

    public async Task CreateAsync(TodoItem entity)
        => await _todoItemRepository.CreateAsync(entity);

    public async Task DeleteAsync(int id)
        => await _todoItemRepository.DeleteAsync(new TodoItem() { TodoItemID = id });

    public async Task<TodoItem> DetailsAsync(TodoItem entity)
        => await _todoItemRepository.DetailAsync(entity);

    public async Task<IEnumerable<TodoItem>> RetrieveAsync(int userID)
        => await _todoItemRepository.RetrieveAsync(new TodoItem() { CreatedBy = new User() { UserID = userID } });

    public async Task UpdateAsync(TodoItem entity)
        => await _todoItemRepository.UpdateAsync(entity);
}
