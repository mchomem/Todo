using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models.Entities;

namespace Todo.Core.Services.Interfaces
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
