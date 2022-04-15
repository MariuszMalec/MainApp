using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Models;
using Tracking.Repositories;

namespace Tracking.Services
{
    public class UserService : IRepositoryService<User>
    {
        private readonly IRepository<User> Persons;

        public UserService(IRepository<User> persons)
        {
            Persons = persons;
        }
        public async Task<IEnumerable<User>> GetAll()
        {
            return await Persons.GetAll();
        }

        public async Task Insert(User person)
        {
            await Persons.Insert(person);
        }

        public async Task<User> Get(int id)
        {
            return await Persons.Get(id);
        }

        public async Task Update(User person)
        {
            await Persons.Update(person);
        }
        public async Task Delete(int id)
        {
            var person = await Persons.Get(id);
            await Persons.Delete(person);
        }
    }
}
