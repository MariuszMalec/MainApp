using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tracking.Models;

namespace Tracking.Repositories
{
    public interface IRepository<T> where T : IBaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetAllQueryable();
        Task<T> Get(int id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
