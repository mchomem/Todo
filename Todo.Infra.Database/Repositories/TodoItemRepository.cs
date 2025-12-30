namespace Todo.Infra.Database.Repositories;

public class TodoItemRepository : ITodoItemRepository
{
    private readonly TodoContext _todoContext;

    public TodoItemRepository(TodoContext todoContext)
        => _todoContext = todoContext;

    public async Task CreateAsync(TodoItem entity)
    {
        _todoContext.TodoItems.Add(entity);
        _todoContext.Users.Attach(entity.CreatedBy);
        _todoContext.Entry(entity.CreatedBy).State = EntityState.Unchanged;
        await _todoContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(TodoItem entity)
    {
        _todoContext.TodoItems.Remove(entity);
        await _todoContext.SaveChangesAsync();
    }

    public async Task DeleteByCreatedUserIdAsync(int createdById)
    {
        _todoContext.TodoItems
            .Where(x => x.CreatedByID == createdById)
            .ToList()
            .ForEach(x => _todoContext.TodoItems.Remove(x));
        
        await _todoContext.SaveChangesAsync();
    }

    public async Task<TodoItem> DetailAsync(TodoItem entity)
        => await _todoContext.TodoItems
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.TodoItemID.Value == entity.TodoItemID.Value);

    public async Task<IEnumerable<TodoItem>> RetrieveAsync(TodoItem entity = null)
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

    public async Task UpdateAsync(TodoItem entity)
    {
        _todoContext.Users.Attach(entity.CreatedBy);
        entity.CreatedByID = entity.CreatedBy.UserID.Value;
        _todoContext.Entry(entity).State = EntityState.Modified;
        await _todoContext.SaveChangesAsync();
    }
}
