using MainApp.BLL.Entities;
using MainApp.BLL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Services
{
    public class TrainerService
    {
        private readonly IRepository<Trainer> Trainers;

        public TrainerService(IRepository<Trainer> users)
        {
            Trainers = users;
        }
        public async Task<IEnumerable<Trainer>> GetAll()
        {
            return await Trainers.GetAll();
        }

        public async Task Insert(Trainer user)
        {
            await Trainers.Insert(user);
        }

        public async Task<Trainer> GetById(int id)
        {
            return await Trainers.GetById(id);
        }

        public async Task Delete(Trainer user)
        {
            await Trainers.Delete(user);
        }
        public async Task Update(Trainer user)
        {
            await Trainers.Update(user);
        }
    }
}
