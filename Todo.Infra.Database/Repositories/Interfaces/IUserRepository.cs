namespace Todo.Infra.Database.Repositories.Interfaces;

public interface IUserRepository : IRepository<User>
{
    public Task<User> AuthenticateAsync(string login, string password);

    public Task ChangePasswordAsync(User entity, string newPassword);
}
