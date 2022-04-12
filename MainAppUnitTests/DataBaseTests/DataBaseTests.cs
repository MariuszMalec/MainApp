using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using System;
using Tracking.Context;
using Tracking.Models;
using Xunit;

namespace MainAppUnitTests.DataBaseTests
{
    public class DataBaseTests
    {
        [Fact]
        public void SuccessLoadingBEntities()
        {
            var connection = new SqliteConnection("Data Source=.\\Database\\MainAppDb.db");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<MainApplicationContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new MainApplicationContext(options))
                {
                    context.Database.EnsureCreated();
                    //init data
                    context.Trainers.Add(new Trainer { Id = 100, FirstName = "trlalal", LastName = "bebeb", CreatedDate=DateTime.Now, Email = "cepek@example.com", PhoneNumber = "222-222-222"});
                    context.SaveChanges();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
