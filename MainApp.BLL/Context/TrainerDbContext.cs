using MainApp.BLL.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MainApp.BLL.Context
{
    public class TrainerDbContext : DbContext
    {
        public TrainerDbContext(DbContextOptions<TrainerDbContext> options) : base(options) { }
        public DbSet<Trainer> Trainesr { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            
        }
    }
}
