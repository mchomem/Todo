namespace Todo.Infrastructure.Persistence.Repositories;

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

    public async Task<TodoItem> GetAsync(TodoItem entity)
    {
        var todoItem = await _todoContext.TodoItems
            .Include(x => x.CreatedBy)
            .FirstOrDefaultAsync(x => x.TodoItemID == entity.TodoItemID);

        return todoItem;
    }

    public async Task<TodoItem> GetAsync(int id)
    {
        var todoItem = await _todoContext.TodoItems
            .Include(x => x.CreatedBy)
            .SingleOrDefaultAsync(x => x.TodoItemID == id);

        return todoItem;
    }

    public async Task<IEnumerable<TodoItem>> GetAllAsync(Expression<Func<TodoItem, bool>> filter
        , IEnumerable<Expression<Func<TodoItem, object>>>? includes = null
        , IEnumerable<(Expression<Func<TodoItem, object>> keySelector, bool asceding)>? orderBy = null)
    {
        IQueryable<TodoItem> query = _todoContext.TodoItems
            .AsQueryable()
            .AsNoTracking()
            .Where(filter);

        if (includes != null)
        {
            foreach (var include in includes)
                query = query.Include(include);
        }

        return await query.ToListAsync();
    }

    public async Task UpdateAsync(TodoItem entity)
    {
        _todoContext.Users.Attach(entity.CreatedBy);
        _todoContext.Entry(entity).State = EntityState.Modified;
        await _todoContext.SaveChangesAsync();
    }
}
