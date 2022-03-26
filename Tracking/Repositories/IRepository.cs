using System.Collections.Generic;
using System.Linq;
using Tracking.Models;

namespace Tracking.Repositories
{
    public interface IRepository<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();
        IQueryable<T> GetAllQueryable();
        T Get(int id);
        void Insert(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
