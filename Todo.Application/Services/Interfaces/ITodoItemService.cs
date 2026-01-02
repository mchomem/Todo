namespace Todo.Application.Services.Interfaces;

public interface ITodoItemService
{
    public Task CreateAsync(TodoItem entity);
    public Task DeleteAsync(int id);
    public Task<TodoItem> DetailsAsync(TodoItem entity);
    public Task<IEnumerable<TodoItem>> RetrieveAsync(int userID);
    public Task UpdateAsync(int id, TodoItem entity);
}
