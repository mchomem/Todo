namespace Todo.Infrastructure.Persistence.Repositories.Interfaces;

public interface ITodoItemRepository : IRepository<TodoItem>
{
    public Task DeleteByCreatedUserIdAsync(int createdById);
}
