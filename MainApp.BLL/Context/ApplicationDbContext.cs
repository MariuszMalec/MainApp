using MainApp.BLL.DataStorage;
using MainApp.BLL.Entities;
using Microsoft.EntityFrameworkCore;

namespace MainApp.BLL.Context
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<Trainer> Trainers { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Event> Events { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)//TODO dodanie uzytkownika do tabeli tylko poprzez migracje!!
        {
            //TODO dodac to jesli baza nie istnieje dodaje do bazy z seed i wlasciwosci do bazy
            //modelBuilder.Seed();//wczytanie wejsciowych danych do bazy z serwisu
            //modelBuilder.ApplyConfigurationsFromAssembly(this.GetType().Assembly);//nakladanie wlasciwosci na baze
        }
    }
}
