namespace Todo.Infrastructure.Persistence.Repositories;

public class UserRepository : IUserRepository
{
    private readonly TodoContext _todoContext;

    public UserRepository(TodoContext todoContext)
    {
        _todoContext = todoContext;
    }

    public async Task<bool> CheckIfExists(Expression<Func<User, bool>> filter)
    {
        IQueryable<User> query = _todoContext.Users
            .AsQueryable()
            .Where(filter)
            .AsNoTracking();

        var result = await query.AnyAsync();
        return result;
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

    public async Task<User> GetAsync(int id)
    {
        var user = await _todoContext.Users
            .Include(x => x.Picture)
            .SingleOrDefaultAsync(x => x.UserID == id);

        return user!;
    }

    public async Task<IEnumerable<User>> GetAllAsync(Expression<Func<User, bool>> filter
        , IEnumerable<Expression<Func<User, object>>>? includes = null
        , IEnumerable<(Expression<Func<User, object>> keySelector, bool asceding)>? orderBy = null)
    {
            IQueryable<User> query = _todoContext.Users
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
                && x.IsActive)
            .SingleOrDefaultAsync();

        return user!;
    }

    public async Task ChangePasswordAsync(User entity)
    {
        await UpdateAsync(entity);
    }
}
