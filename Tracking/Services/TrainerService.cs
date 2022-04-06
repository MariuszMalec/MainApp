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

        public Trainer Get(int id)
        {
            return Persons.Get(id);
        }

        public void Update(Trainer person)
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
