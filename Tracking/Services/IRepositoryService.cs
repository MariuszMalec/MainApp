using System.Collections.Generic;
using Tracking.Models;

namespace Tracking.Services
{
    public interface IRepositoryService<T> where T : BaseEntity
    {
        IEnumerable<T> GetAll();

        void Insert(T person);

        T Get(int id);

        void Update(T person);

        void Delete(int id);
    }
}
