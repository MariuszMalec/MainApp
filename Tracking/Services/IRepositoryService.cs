using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Models;

namespace Tracking.Services
{
    public interface IRepositoryService<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();

        Task Insert(T person);

        Task<T> Get(int id);

        Task Update(T person);

        Task Delete(int id);

        Task<AuthenticateModel> Authenticate(string email);
    }
}
