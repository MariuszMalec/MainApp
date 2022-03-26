using System.Collections.Generic;
using Tracking.Models;
using Tracking.Repositories;

namespace Tracking.Services
{
    public class TrainerService
    {
        private readonly IRepository<Trainer> Users;

        public TrainerService(IRepository<Trainer> users)
        {
            Users = users;
        }
        public IEnumerable<Trainer> GetAll()
        {
            return Users.GetAll();
        }

        public void Insert(Trainer user)
        {
            Users.Insert(user);
        }

        public User Get(int id)
        {
            return Users.Get(id);
        }

        public void Update(Trainer user)
        {
            Users.Update(user);
        }
        public void Delete(int id)
        {
            var user = Users.Get(id);
            Users.Delete(user);
        }
    }
}
