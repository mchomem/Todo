using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        private readonly TodoContext _todoContext;

        public TodoItemRepository(TodoContext todoContext)
        {
            _todoContext = todoContext;
        }

        public async Task Create(TodoItem entity)
        {
            _todoContext.TodoItems.Add(entity);
            _todoContext.Users.Attach(entity.CreatedBy);
            _todoContext.Entry(entity.CreatedBy).State = EntityState.Unchanged;
            await _todoContext.SaveChangesAsync();
        }

        public async Task Delete(TodoItem entity)
        {
            _todoContext.TodoItems.Remove(entity);
            await _todoContext.SaveChangesAsync();
        }

        public async Task<TodoItem> Details(TodoItem entity)
        {
            return await _todoContext.TodoItems
                .Include(x => x.CreatedBy)
                .FirstOrDefaultAsync(x => x.TodoItemID.Value == entity.TodoItemID.Value);
        }

        public async Task<IEnumerable<TodoItem>> Retrieve(TodoItem entity = null)
        {
            if (entity != null)
                return await _todoContext.TodoItems
                    .Include(x => x.CreatedBy)
                    .Where(x =>
                    (
                        (!entity.TodoItemID.HasValue || x.TodoItemID.Value == entity.TodoItemID.Value)
                        && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                        && (!entity.IsDone.HasValue || x.IsDone.Value == entity.IsDone.Value)
                        && (!entity.DeadLine.HasValue || x.DeadLine.Value == entity.DeadLine.Value)
                        && (entity.CreatedBy == null || x.CreatedBy.UserID == entity.CreatedBy.UserID)
                    ))
                    .ToListAsync();
            else
                return await _todoContext.TodoItems
                    .Include(x => x.CreatedBy)
                    .ToListAsync();
        }

        public async Task Update(TodoItem entity)
        {
            _todoContext.Users.Attach(entity.CreatedBy);
            entity.CreatedByID = entity.CreatedBy.UserID.Value;
            _todoContext.Entry(entity).State = EntityState.Modified;
            await _todoContext.SaveChangesAsync();
        }
    }
}
