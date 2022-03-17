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
        public async Task Create(TodoItem entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.TodoItems.Add(entity);
                db.Users.Attach(entity.CreatedBy);
                db.Entry(entity.CreatedBy).State = EntityState.Unchanged;
                await db.SaveChangesAsync();
            }
        }

        public async Task Delete(TodoItem entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.TodoItems.Remove(entity);
                await db.SaveChangesAsync();
            }
        }

        public async Task<TodoItem> Details(TodoItem entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return await db.TodoItems
                    .Include(x => x.CreatedBy)
                    .FirstOrDefaultAsync(x => x.TodoItemID.Value == entity.TodoItemID.Value);
            }
        }

        public async Task<IEnumerable<TodoItem>> Retrieve(TodoItem entity = null)
        {
            using (TodoContext db = new TodoContext())
            {
                if (entity != null)
                    return await db.TodoItems
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
                    return await db.TodoItems
                        .Include(x => x.CreatedBy)
                        .ToListAsync();
            }
        }

        public async Task Update(TodoItem entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Attach(entity.CreatedBy);
                entity.CreatedByID = entity.CreatedBy.UserID.Value;
                db.Entry(entity).State = EntityState.Modified;
                await db.SaveChangesAsync();
            }
        }
    }
}
