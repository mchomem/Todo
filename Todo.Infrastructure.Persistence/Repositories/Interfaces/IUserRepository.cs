namespace Todo.Infrastructure.Persistence.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public Task<User> AuthenticateAsync(string login, string password);
    public Task ChangePasswordAsync(User entity);
    public Task<bool> CheckIfExists(Expression<Func<User, bool>> filter);
}
