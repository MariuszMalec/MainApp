using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tracking.Models;

namespace Tracking.Context
{
    public static class TrainerSeed
    {
        private static List<Trainer> Trainers = new List<Trainer>()//TODO insert startowe dane do bazy
        {
            new Trainer {Id = 1, FirstName = "Patryk", LastName="Szwermer" , Email ="pssg@example.com", PhoneNumber = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 2, FirstName = "Przemyslaw", LastName="Sawicki" , Email ="ps@example.com", PhoneNumber = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 3, FirstName = "Marcin", LastName="Dabrowski" , Email ="md@example.com", PhoneNumber = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 4, FirstName = "Piotr", LastName="Katny" , Email ="pk@example.com", PhoneNumber = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 5, FirstName = "Marcin", LastName="Dudzic" , Email ="md@example.com", PhoneNumber = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 6, FirstName = "Maciej", LastName="Krakowiak" , Email ="mk@example.com", PhoneNumber = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 7, FirstName = "Mateusz", LastName="Cebula" , Email ="mc@example.com", PhoneNumber = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 8, FirstName = "Jakub", LastName="Nowikowski" , Email ="jk@example.com", PhoneNumber = "" , CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 9, FirstName = "Jan", LastName="Choma" , Email ="jc@example.com", PhoneNumber = "" , CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 10, FirstName = "Marcin", LastName="Przypek" , Email ="mp@example.com", PhoneNumber = "" , CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 11, FirstName = "Michal", LastName="Sosnowski" , Email ="ms@example.com", PhoneNumber = "" , CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 12, FirstName = "Maciej", LastName="Tyszka" , Email ="mt@example.com", PhoneNumber = "" , CreatedDate = DateTime.UtcNow}
        };

        public static List<Trainer> GetAll()
        {
            return Trainers;
        }
        public static void Seed(this ModelBuilder modelBuilder)//przez MainApplicationContext gdy odpalimy OnModelCreating... 
        {
            modelBuilder.Entity<Trainer>().HasData(
                GetAll()
                );
        }

        public static async void SeedTrainer(MainApplicationContext context)
        {
            if (context.Trainers.Any())
            {
                return;
            }

            var trainer = new Trainer()
            {
                CreatedDate = DateTime.UtcNow,
                Email = "Admin@example.com",
                FirstName = "Admin",
                LastName = "Admin",
                PhoneNumber = "22222"
            };
            context.AddRange(trainer);
            await context.SaveChangesAsync();
        }

    }
}
