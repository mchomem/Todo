namespace Todo.Infra.Database.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TodoContext _todoContext;

    public UserRepository(TodoContext todoContext)
    {
        _todoContext = todoContext;
    }

    public async Task CreateAsync(User entity)
    {
        _todoContext.Users.Add(entity);
        await _todoContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(User entity)
    {
        _todoContext.Users.Remove(entity);
        await _todoContext.SaveChangesAsync();
    }

    public async Task<User> GetAsync(User entity)
    {
        var user = await _todoContext.Users
            .Include(x => x.Picture)
                .FirstOrDefaultAsync(x => x.UserID == entity.UserID);

        return user!;
    }

    public async Task<IEnumerable<User>> GetAllAsync(User entity)
    {
        
        var users = await _todoContext.Users
                // TODO: transfer this where clause to Service layer.
                .Where(x =>
                (
                    (!entity.UserID.HasValue || x.UserID.Value == entity.UserID.Value)
                    && (string.IsNullOrEmpty(entity.Name) || x.Name.Contains(entity.Name))
                    && (string.IsNullOrEmpty(entity.Login) || x.Name.Contains(entity.Login))
                    && (!entity.IsActive.HasValue || x.IsActive.Value == entity.IsActive.Value)
                ))
                .ToListAsync();
        // TODO: apply paggination with take and skip and a object to represent a paggination.

        return users;
    }

    public async Task UpdateAsync(User entity)
    {
        if (entity.Picture != null)
        {
            _todoContext.UserPictures.Attach(entity.Picture);
            _todoContext.Entry(entity.Picture).State = EntityState.Unchanged;
        }

        _todoContext.Users.Update(entity);
        await _todoContext.SaveChangesAsync();
    }

    public async Task<User> AuthenticateAsync(string login, string password)
    {
        var user = await _todoContext.Users
            .Include(x => x.Picture)
            .Where(x =>
                x.Login == login
                && x.Password == password
                && x.IsActive.Value)
            .SingleOrDefaultAsync();

        return user!;
    }

    public async Task ChangePasswordAsync(User entity)
    {
        await UpdateAsync(entity);
    }
}
