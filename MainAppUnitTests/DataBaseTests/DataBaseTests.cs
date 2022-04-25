using FluentAssertions;
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
        public void CheckDataBase_ReturnError_WhenNotTrainersExist()
        {
            var connection = new SqliteConnection("Data Source=C:\\temp\\Databases\\MainAppDb.db");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<MainApplicationContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new MainApplicationContext(options))
                {
                    connection.Close();

                    //delete before test
                    context.Database.EnsureDeleted();

                    connection.Open();

                    //create again
                    context.Database.EnsureCreated();

                    //init data
                    var any = context.Trainers.AnyAsync();
                    //assert
                    any.Result.Should().Be(true);
                }
            }
            finally
            {
                connection.Close();
            }
        }

        [Fact]
        public void AddTrainer_ShoudReturnError_WhenUserIdIsDuplicated()
        {
            var connection = new SqliteConnection("Data Source=C:\\temp\\Databases\\MainAppDb.db");
            connection.Open();

            try
            {
                var options = new DbContextOptionsBuilder<MainApplicationContext>()
                    .UseSqlite(connection)
                    .Options;

                using (var context = new MainApplicationContext(options))
                {
                    var userId = 104;

                    connection.Close();

                    //delete before test
                    context.Database.EnsureDeleted();

                    connection.Open();

                    //create again
                    context.Database.EnsureCreated();

                    //init data
                    context.Trainers.Add(new Trainer { Id = userId, FirstName = "Test", LastName = "Jednostkowy", CreatedDate=DateTime.Now, Email = "cepek@example.com", PhoneNumber = "222-222-222"});
                    
                    //Act
                    Action act = () => context.SaveChanges();

                    //assert
                    act.Should().NotThrow<DbUpdateException>();
                }
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
