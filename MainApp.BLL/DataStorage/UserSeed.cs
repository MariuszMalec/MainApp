using MainApp.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MainApp.BLL.DataStorage
{
    public static class TrainerSeed
    {
        private static IEnumerable<Trainer> _trainers = new List<Trainer>()//TODO insert startowe dane do bazy
        {
            new Trainer {Id = 1, FirstName = "Patryk", LastName="Szwermer" , Email ="", PhoneNumber = ""},
            new Trainer {Id = 2, FirstName = "Przemyslaw", LastName="sawicki" , Email ="", PhoneNumber = ""},
            new Trainer {Id = 3, FirstName = "Marcin", LastName="Dabrowski" , Email ="", PhoneNumber = ""},
            new Trainer {Id = 4, FirstName = "Piotr", LastName="Katny" , Email ="", PhoneNumber = ""},
            new Trainer {Id = 5, FirstName = "Marcin", LastName="Dudzic" , Email ="", PhoneNumber = ""},
            new Trainer {Id = 6, FirstName = "Maciej", LastName="Krakowiak" , Email ="", PhoneNumber = ""},
            new Trainer {Id = 7, FirstName = "Jakub", LastName="Nowikowski" , Email ="", PhoneNumber = "" }
        };

        public static IEnumerable<Trainer> GetAll()
        {
            return _trainers;
        }

        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainer>().HasData(
                GetAll()
                );
        }
    }
}
