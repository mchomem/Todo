namespace Todo.Infra.Database.Repositories.Interfaces;

public interface IRepository<E> where E : class
{
    public Task CreateAsync(E entity);
    public Task DeleteAsync(E entity);
    public Task<E> GetAsync(E entity);
    public Task<IEnumerable<E>> GetAllAsync(E entity);
    public Task UpdateAsync(E entity);
}
