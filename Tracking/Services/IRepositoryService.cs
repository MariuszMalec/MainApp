using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Models;

namespace Tracking.Services
{
    public interface IRepositoryService<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();

        Task Insert(T person);

        T Get(int id);

        void Update(T person);

        void Delete(int id);
    }
}
