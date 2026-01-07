namespace Todo.Infra.Database.Repositories.Interfaces;

public interface IRepository<TEntity> where TEntity : class
{
    public Task CreateAsync(TEntity entity);

    public Task DeleteAsync(TEntity entity);

    public Task<TEntity> GetAsync(int id);

    public Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter
        , IEnumerable<Expression<Func<TEntity, object>>>? includes = null
        , IEnumerable<(Expression<Func<TEntity, object>> keySelector, bool asceding)>? orderBy = null);
    
    public Task UpdateAsync(TEntity entity);
}
