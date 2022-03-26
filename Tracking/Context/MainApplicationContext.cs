using Microsoft.EntityFrameworkCore;
using Tracking.Models;

namespace Tracking.Context
{
    public class MainApplicationContext : DbContext
    {
        public DbSet<Trainer> Trainers { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<User> Users { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Data Source=.\\Database\\MainAppDb.db");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Trainer>().ToTable("Trainers");
            modelBuilder.Seed();
            modelBuilder.Entity<User>().ToTable("Users");
            modelBuilder.Entity<Event>().ToTable("Events");      
        }
    }
}
