using MainApp.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace MainApp.BLL.DataStorage
{
    public static class TrainerSeed
    {
        private static IEnumerable<Trainer> _trainers = new List<Trainer>()//TODO insert startowe dane do bazy
        {
            new Trainer {Id = 1, FirstName = "Patryk", LastName="Szwermer" , Email ="", Phone = ""},
            new Trainer {Id = 2, FirstName = "Przemyslaw", LastName="sawicki" , Email ="", Phone = ""},
            new Trainer {Id = 3, FirstName = "Marcin", LastName="Dabrowski" , Email ="", Phone = ""},
            new Trainer {Id = 4, FirstName = "Piotr", LastName="Katny" , Email ="", Phone = ""},
            new Trainer {Id = 5, FirstName = "Marcin", LastName="Dudzic" , Email ="", Phone = ""},
            new Trainer {Id = 6, FirstName = "Maciej", LastName="Krakowiak" , Email ="", Phone = ""},
            new Trainer {Id = 7, FirstName = "Jakub", LastName="Nowikowski" , Email ="", Phone = "" }
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
