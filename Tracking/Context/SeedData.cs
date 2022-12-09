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
                CreatedDate = DateTime.UtcNow,
                Email = "Trainer@example.com",
                FirstName = "Trainer",
                LastName = "Trainer",
                PhoneNumber = "505505501"
            };
            context.AddRange(trainer);
            await context.SaveChangesAsync();
        }

        public static async void SeedUser(MainApplicationContext context)
        {
            if (context.Users.Any())
            {
                return;
            }

            var user = new User()
            {
                CreatedDate = DateTime.UtcNow,
                Email = "Admin@example.com",
                FirstName = "Admin",
                LastName = "Admin",
                PhoneNumber = "505505502"
            };
            context.AddRange(user);
            await context.SaveChangesAsync();
        }

        public static async void SeedEvent(MainApplicationContext context)
        {
            if (context.Events.Any())
            {
                return;
            }

            context.AddRange(new Event() { CreatedDate = DateTime.UtcNow,
                                           UserId = 1,
                                           Email= "Admin@example.com",
                                           Action = "register"
            });
            await context.SaveChangesAsync();

        }
    }
}
