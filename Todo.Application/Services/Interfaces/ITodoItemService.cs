namespace Todo.Application.Services.Interfaces;

public interface ITodoItemService
{
    public Task CreateAsync(TodoItemInsertDto todoItemDto);
    public Task DeleteAsync(int id);
    public Task<TodoItemDto> GetAsync(int id);
    public Task<IEnumerable<TodoItemDto>> GetAllByUserIdAsync(int userID);
    public Task UpdateAsync(int id, TodoItemUpdateDto entity);
}
