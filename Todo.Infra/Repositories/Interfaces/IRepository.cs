namespace Todo.Infra.Repositories.Interfaces
{
    public interface IRepository<E> where E : class
    {
        public Task CreateAsync(E entity);
        public Task DeleteAsync(E entity);
        public Task<E> DetailAsync(E entity);
        public Task<IEnumerable<E>> RetrieveAsync(E entity);
        public Task UpdateAsync(E entity);
    }
}
