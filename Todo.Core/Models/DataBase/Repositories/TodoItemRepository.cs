using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using Todo.Core.Models.DataBase.Repositories.Interfaces;
using Todo.Core.Models.Entities;

namespace Todo.Core.Models.DataBase.Repositories
{
    public class TodoItemRepository : ITodoItemRepository
    {
        public void Create(TodoItem entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.TodoItems.Add(entity);
                db.Users.Attach(entity.CreatedBy);
                db.Entry(entity.CreatedBy).State = EntityState.Unchanged;
                db.SaveChanges();
            }
        }

        public void Delete(TodoItem entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.TodoItems.Remove(entity);
                db.SaveChanges();
            }
        }

        public TodoItem Details(TodoItem entity)
        {
            using (TodoContext db = new TodoContext())
            {
                return db.TodoItems
                    .Include(x => x.CreatedBy)
                    .FirstOrDefault(x => x.TodoItemID.Value == entity.TodoItemID.Value);
            }
        }

        public List<TodoItem> Retrieve(TodoItem entity = null)
        {
            using (TodoContext db = new TodoContext())
            {
                if (entity != null)
                    return db.TodoItems
                        .Include(x => x.CreatedBy)
                        .Where(x =>
                        (
                            (!entity.TodoItemID.HasValue || x.TodoItemID.Value == entity.TodoItemID.Value)
                            && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                            && (!entity.IsDone.HasValue || x.IsDone.Value == entity.IsDone.Value)
                            && (!entity.DeadLine.HasValue || x.DeadLine.Value == entity.DeadLine.Value)
                            && (entity.CreatedBy == null || x.CreatedBy.UserID == entity.CreatedBy.UserID)
                        ))
                        .ToList();
                else
                    return db.TodoItems
                        .Include(x => x.CreatedBy)
                        .ToList();
            }
        }

        public void Update(TodoItem entity)
        {
            using (TodoContext db = new TodoContext())
            {
                db.Users.Attach(entity.CreatedBy);
                entity.CreatedByID = entity.CreatedBy.UserID.Value;
                db.Entry(entity).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}
