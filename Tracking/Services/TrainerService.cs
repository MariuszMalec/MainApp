using System.Collections.Generic;
using System.Threading.Tasks;
using Tracking.Models;
using Tracking.Repositories;

namespace Tracking.Services
{
    public class TrainerService : IRepositoryService<Trainer>
    {
        private readonly IRepository<Trainer> Persons;

        public TrainerService(IRepository<Trainer> persons)
        {
            Persons = persons;
        }
        public async Task<IEnumerable<Trainer>> GetAll()
        {
            return await Persons.GetAll();
        }

        public async Task Insert(Trainer person)
        {
            await Persons.Insert(person);
        }

        public async Task<Trainer> Get(int id)
        {
            return await Persons.Get(id);
        }

        public async Task Update(Trainer person)
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
