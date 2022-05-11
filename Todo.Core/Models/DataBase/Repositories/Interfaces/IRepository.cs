using System.Collections.Generic;
using System.Threading.Tasks;

namespace Todo.Core.Models.DataBase.Repositories.Interfaces
{
    public interface IRepository<E> where E : class
    {
        public Task Create(E entity);
        public Task Delete(E entity);
        public Task<E> Details(E entity);
        public Task<IEnumerable<E>> Retrieve(E entity);
        public Task Update(E entity);
    }
}
