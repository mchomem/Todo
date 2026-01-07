namespace Todo.Infra.Database.Repositories;

public class UserPictureRepository : IUserPictureRepository
{
    private readonly TodoContext _todoContext;

    public UserPictureRepository(TodoContext todoContext)
        => _todoContext = todoContext;

    public async Task CreateAsync(UserPicture entity)
    {
        _todoContext.UserPictures.Add(entity);
        _todoContext.Users.Attach(entity.User);
        _todoContext.Entry(entity.User).State = EntityState.Unchanged;
        await _todoContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(UserPicture entity)
    {
        _todoContext.Users.Attach(entity.User);
        _todoContext.Entry(entity.User).State = EntityState.Unchanged;
        _todoContext.UserPictures.Remove(entity);
        await _todoContext.SaveChangesAsync();
    }

    public async Task<UserPicture> GetByUserId(int userId)
    {
        var userPicture = await _todoContext.UserPictures
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.PictureFromUserID == userId);
        return userPicture!;
    }

    public async Task<UserPicture> GetAsync(int id)
    {
        var userPicture = await _todoContext.UserPictures
            .Include(x => x.User)
            .SingleOrDefaultAsync(x => x.UserPictureID == id);
        return userPicture!;
    }

    public async Task<IEnumerable<UserPicture>> GetAllAsync(Expression<Func<UserPicture, bool>> filter
        , IEnumerable<Expression<Func<UserPicture, object>>>? includes = null
        , IEnumerable<(Expression<Func<UserPicture, object>> keySelector, bool asceding)>? orderBy = null)
    {
        IQueryable<UserPicture> query = _todoContext.UserPictures
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

    public async Task UpdateAsync(UserPicture entity)
    {
        _todoContext.Users.Attach(entity.User);
        _todoContext.Entry(entity.User).State = EntityState.Unchanged;
        _todoContext.UserPictures.Update(entity);
        await _todoContext.SaveChangesAsync();
    }
}
