using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using Tracking.Models;

namespace Tracking.Context
{
    public static class TrainerSeed
    {
        private static List<Trainer> Trainers = new List<Trainer>()//TODO insert startowe dane do bazy
        {
            new Trainer {Id = 1, FirstName = "Patryk", LastName="Szwermer" , Email ="ps@example.com", Phone = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 2, FirstName = "Przemyslaw", LastName="sawicki" , Email ="ps@example.com", Phone = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 3, FirstName = "Marcin", LastName="Dabrowski" , Email ="md@example.com", Phone = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 4, FirstName = "Piotr", LastName="Katny" , Email ="pk@example.com", Phone = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 5, FirstName = "Marcin", LastName="Dudzic" , Email ="md@example.com", Phone = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 6, FirstName = "Maciej", LastName="Krakowiak" , Email ="mk@example.com", Phone = "", CreatedDate = DateTime.UtcNow},
            new Trainer {Id = 7, FirstName = "Jakub", LastName="Nowikowski" , Email ="jk@example.com", Phone = "" , CreatedDate = DateTime.UtcNow}
        };

        public static List<Trainer> GetAll()
        {
            return Trainers;
        }
        public static void Seed(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainer>().HasData(
                GetAll()
                );
        }

    }
}
