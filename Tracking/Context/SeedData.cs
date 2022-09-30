using System;
using System.Linq;
using Tracking.Models;

namespace Tracking.Context
{
    public class SeedData
    {
        public static async void SeedTrainer(MainApplicationContext context)
        {
            if (context.Trainers.Any())
            {
                return;
            }

            var trainer = new Trainer()
            {
                CreatedDate = DateTime.Now,
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
