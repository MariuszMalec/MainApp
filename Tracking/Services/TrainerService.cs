﻿using System.Collections.Generic;
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
        public IEnumerable<Trainer> GetAll()
        {
            return Persons.GetAll();
        }

        public void Insert(Trainer person)
        {
            Persons.Insert(person);
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
