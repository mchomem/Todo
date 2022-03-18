using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Entities;
using Todo.Core.Services.Interfaces;

namespace Todo.Core.Services
{
    public class TodoItemService : ITodoItemService
    {
        private readonly ITodoItemRepository _todoItemRepository;

        public TodoItemService(ITodoItemRepository todoItemRepository)
        {
            _todoItemRepository = todoItemRepository;
        }

        public async Task CreateAsync(TodoItem entity)
        {
            await _todoItemRepository.Create(entity);
        }

        public async Task DeleteAsync(int id)
        {
            await _todoItemRepository.Delete(new TodoItem() { TodoItemID = id });
        }

        public async Task<TodoItem> DetailsAsync(TodoItem entity)
        {
            return await _todoItemRepository.Details(entity);
        }

        public async Task<IEnumerable<TodoItem>> RetrieveAsync(int userID)
        {
            return await _todoItemRepository
                .Retrieve(new TodoItem() { CreatedBy = new User() { UserID = userID } });
        }

        public async Task UpdateAsync(TodoItem entity)
        {
            await _todoItemRepository.Update(entity);
        }
    }
}
