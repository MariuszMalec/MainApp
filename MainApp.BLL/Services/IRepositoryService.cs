using MainApp.BLL.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public interface IRepositoryService<T> where T : class
    {
        Task<bool> Delete(int id, T entity);
        Task<IEnumerable<T>> GetAll();
        IQueryable<T> GetAllQueryable();
        Task<T> GetById(int id);
        Task<bool> Insert(T entity);
        Task Update(T entity);
    }
}