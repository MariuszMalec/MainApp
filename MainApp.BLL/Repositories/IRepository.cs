using MainApp.BLL.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.BLL.Repositories
{
    public interface IRepository<T> where T : IEntity
    {
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetAllQueryable();
        Task<T> GetById(int id);
        Task Insert(T entity);
        Task Update(T entity);
        Task Delete(T entity);
    }
}
