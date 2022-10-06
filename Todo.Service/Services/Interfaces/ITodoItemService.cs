using Todo.Domain.Entities;

namespace Todo.Service.Services.Interfaces
{
    public interface ITodoItemService
    {
        public Task CreateAsync(TodoItem entity);
        public Task DeleteAsync(int id);
        public Task<TodoItem> DetailsAsync(TodoItem entity);
        public Task<IEnumerable<TodoItem>> RetrieveAsync(int userID);
        public Task UpdateAsync(TodoItem entity);
    }
}
