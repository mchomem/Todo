namespace Todo.Infra.Database.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public Task<User> AuthenticateAsync(User entity);

    public Task ChangePasswordAsync(User entity, string newPassword);
}
