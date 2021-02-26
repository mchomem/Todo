using System.Collections.Generic;

namespace Todo.Core.Models.DataBase.Repositories.Interfaces
{
    public interface ICrud<E> where E : class
    {
        public void Create(E entity);
        public void Delete(E entity);
        public E Details(E entity);
        public List<E> Retrieve(E entity);
        public void Update(E entity);
    }
}
