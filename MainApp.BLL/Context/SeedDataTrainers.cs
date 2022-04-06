using MainApp.BLL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace MainApp.BLL.Context
{
    public class SeedDataTrainers
    {
        private static List<Trainer> Trainers = new List<Trainer>()//TODO insert startowe dane do bazy
        {
            new Trainer {Id = 1, FirstName = "Patryk", LastName="Szwermer" , Email ="ps@example.com", PhoneNumber = "", CreatedDate = DateTime.Now},
            new Trainer {Id = 2, FirstName = "Przemyslaw", LastName="sawicki" , Email ="ps@example.com", PhoneNumber = "", CreatedDate = DateTime.Now},
            new Trainer {Id = 3, FirstName = "Marcin", LastName="Dabrowski" , Email ="md@example.com", PhoneNumber = "", CreatedDate = DateTime.Now},
            new Trainer {Id = 4, FirstName = "Piotr", LastName="Katny" , Email ="pk@example.com", PhoneNumber = "", CreatedDate = DateTime.Now},
            new Trainer {Id = 5, FirstName = "Marcin", LastName="Dudzic" , Email ="md@example.com", PhoneNumber = "", CreatedDate = DateTime.Now},
            new Trainer {Id = 6, FirstName = "Maciej", LastName="Krakowiak" , Email ="mk@example.com", PhoneNumber = "", CreatedDate = DateTime.Now},
            new Trainer {Id = 7, FirstName = "Jakub", LastName="Nowikowski" , Email ="jk@example.com", PhoneNumber = "" , CreatedDate = DateTime.Now}
        };
        public static async void Seed(ApplicationDbContext context)
        {
            if (context.Users.Any())
            {
                return;
            }
            context.AddRange(Trainers);
            await context.SaveChangesAsync();
        }

    }
}
