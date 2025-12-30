namespace Todo.Infra.Database.Repositories.Interfaces;

public interface ITodoItemRepository : IRepository<TodoItem>
{
    public Task DeleteByCreatedUserIdAsync(int createdById);
}
