using System.Collections.Generic;
using Tracking.Models;
using Tracking.Repositories;

namespace Tracking.Services
{
    public class TrainerService
    {
        private readonly IRepository<Trainer> Trainers;

        public TrainerService(IRepository<Trainer> trainers)
        {
            Trainers = trainers;
        }
        public IEnumerable<Trainer> GetAll()
        {
            return Trainers.GetAll();
        }

        public void Insert(Trainer user)
        {
            Trainers.Insert(user);
        }

        public Trainer Get(int id)
        {
            return Trainers.Get(id);
        }

        public void Update(Trainer user)
        {
            Trainers.Update(user);
        }
        public void Delete(int id)
        {
            var user = Trainers.Get(id);
            Trainers.Delete(user);
        }
    }
}
