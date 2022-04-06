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

        public User Get(int id)
        {
            return Persons.Get(id);
        }

        public void Update(User person)
        {
            Persons.Update(person);
        }
        public void Delete(int id)
        {
            var person = Persons.Get(id);
            Persons.Delete(person);
        }
    }
}
