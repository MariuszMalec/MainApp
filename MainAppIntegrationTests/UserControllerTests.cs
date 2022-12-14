using FluentAssertions;
using MainApp.BLL.Context;
using MainApp.Web;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace MainAppIntegrationTests
{
    public class UserControllerTests : IClassFixture<WebApplicationFactory<Program>>//wspoldzielenie factory testy nieco szybsze
    {
        private HttpClient _client;
        private string _repoUser = Path.Combine(@"C:\Users", Environment.UserName, @"source\repos\MainApp\MainApp.Web\DataBaseUser", "TestMainAppUsersDb.db");

        public UserControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        var dbContextOptions = services
                            .SingleOrDefault(service => service.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                        services.Remove(dbContextOptions);

                        ;

                        services.AddDbContext<ApplicationDbContext>(o => o.UseSqlite($"Data Source={_repoUser}"));

                    });
                })
                .CreateClient();
            //_client = factory.CreateClient();//TODO blad sql 14!!
        }

        [Fact]
        public async Task GetAll_Users_ReturnOk_WhenExist()
        {

            //act
            var response = await _client.GetAsync("/User");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetAll_Users_ReturnNotFound_WhenNotExist()
        {

            //act
            var response = await _client.GetAsync("/Userek");

            //assert
            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }
    }
}
